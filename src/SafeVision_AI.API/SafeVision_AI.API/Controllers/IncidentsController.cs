using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SafeVision_AI.API.Data;
using SafeVision_AI.API.Hubs;
using SafeVision_AI.API.Interfaces;
using SafeVision_AI.API.Services;
using SafeVisionAI.Core.Entities;
using SafeVisionAI.Core.Enums;
using SafeVisionAI.Infrastructure.DTOs;

namespace SafeVision_AI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly SafeVisionDbContext _db;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IEmailNotificationService _emailNotificationService;

        public IncidentsController(SafeVisionDbContext db, IEmailNotificationService emailNotificationService, IHubContext<NotificationHub> hubContext)
        {
            _db = db;
            _emailNotificationService = emailNotificationService;
            _hubContext = hubContext;
        }
        [HttpGet("all")]
        public IActionResult GetAllIncidents()
        {
            try
            {
                var incidents = _db.Incidents
                    .Include(i => i.Camera)
                    .OrderByDescending(i => i.DetectedAt)
                    .Select(i => new IncidentDto
                    {
                        Id = i.Id,
                        CameraId = i.CameraId,
                        CameraName = i.Camera.Name,
                        CameraLocation = i.Camera.Location,
                        Type = i.Type.ToString(),
                        Description = i.Description,
                        ConfidenceScore = i.ConfidenceScore,
                        Severity = i.Severity.ToString(),
                        DetectedAt = i.DetectedAt,
                        IsResolved = i.IsResolved,
                        ResolvedBy = i.ResolvedBy,
                        ResolvedAt = i.ResolvedAt,
                        Notes = i.Notes,
                        VideoClipUrl = i.VideoClipUrl,
                        ImageUrl = i.ImageUrl,
                        BoundingBox = i.BoundingBoxX.HasValue ? new BoundingBoxDto
                        {
                            X = i.BoundingBoxX.Value,
                            Y = i.BoundingBoxY.Value,
                            Width = i.BoundingBoxWidth.Value,
                            Height = i.BoundingBoxHeight.Value
                        } : null
                    })
                    .ToList();

                return Ok(new ApiResponse<List<IncidentDto>>
                {
                    Success = true,
                    Message = "Incidents fetched successfully",
                    Data = incidents
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to fetch incidents",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetIncidentById(int id)
        {
            try
            {
                var incident = _db.Incidents
                    .Include(i => i.Camera)
                    .Where(i => i.Id == id)
                    .Select(i => new IncidentDto
                    {
                        Id = i.Id,
                        CameraId = i.CameraId,
                        CameraName = i.Camera.Name,
                        CameraLocation = i.Camera.Location,
                        Type = i.Type.ToString(),
                        Description = i.Description,
                        ConfidenceScore = i.ConfidenceScore,
                        Severity = i.Severity.ToString(),
                        DetectedAt = i.DetectedAt,
                        IsResolved = i.IsResolved,
                        ResolvedBy = i.ResolvedBy,
                        ResolvedAt = i.ResolvedAt,
                        Notes = i.Notes,
                        VideoClipUrl = i.VideoClipUrl,
                        ImageUrl = i.ImageUrl,
                        BoundingBox = i.BoundingBoxX.HasValue ? new BoundingBoxDto
                        {
                            X = i.BoundingBoxX.Value,
                            Y = i.BoundingBoxY.Value,
                            Width = i.BoundingBoxWidth.Value,
                            Height = i.BoundingBoxHeight.Value
                        } : null
                    })
                    .FirstOrDefault();

                if (incident == null)
                    return NotFound(new ApiResponse<string> { Success = false, Message = "Incident not found" });

                return Ok(new ApiResponse<IncidentDto> { Success = true, Data = incident });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to retrieve incident",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // POST: api/incident/add
        [HttpPost("add")]
        public async Task<IActionResult> AddIncident([FromBody] IncidentDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Invalid data" });

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                Incident incident = new Incident();
                incident.CameraId = dto.CameraId;

                if (Enum.TryParse<IncidentType>(dto.Type, out var incidentType))
                {
                    incident.Type = incidentType;
                }
                else
                {
                    return BadRequest(new ApiResponse<string> { Success = false, Message = "Invalid incident type" });
                }

                incident.Description = dto.Description;
                incident.ConfidenceScore = dto.ConfidenceScore;
                incident.Severity = Enum.TryParse<IncidentSeverity>(dto.Severity, out var severity) ? severity : IncidentSeverity.Unknown;
                incident.DetectedAt = dto.DetectedAt;
                incident.VideoClipUrl = dto.VideoClipUrl;
                incident.ImageUrl = dto.ImageUrl;
                incident.BoundingBoxX = dto.BoundingBox?.X;
                incident.BoundingBoxY = dto.BoundingBox?.Y;
                incident.BoundingBoxWidth = dto.BoundingBox?.Width;
                incident.BoundingBoxHeight = dto.BoundingBox?.Height;
                incident.Notes = dto.Notes;

                _db.Incidents.Add(incident);
                await _db.SaveChangesAsync();

                // Explicitly load the Camera navigation property
                await _db.Entry(incident).Reference(i => i.Camera).LoadAsync();

                if (incident.Severity == IncidentSeverity.Critical || incident.Severity == IncidentSeverity.High)
                {
                    var alert = new IncidentAlert
                    {
                        IncidentId = incident.Id,
                        RecipientEmail = "saadse786@gmail.com",
                        RecipientPhone = "03002231675",
                        Status = AlertStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };
                    _db.IncidentAlerts.Add(alert);
                    await _db.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("ReceiveAlert", new
                    {
                        IncidentId = incident.Id,
                        Camera = incident.Camera.Name ?? "Default",
                        Type = incident.Type.ToString(),
                        Severity = incident.Severity.ToString(),
                        TimeStamp = incident.DetectedAt,
                        Location = incident.Camera.Location ?? "Default Location"
                    });

                    var subject = $"[SafeVision AI] {incident.Type} Alert - {incident.Camera?.Location ?? "Unknown Location"}";
                    var body = EmailBodyService.GenerateAlertEmailBody(incident, alert);
                    var emailSent = await _emailNotificationService.SendEmail(alert.RecipientEmail, subject, body);
                    if (emailSent)
                    {
                        alert.Status = AlertStatus.Sent;
                        alert.SentAt = DateTime.UtcNow;
                    }
                    else
                    {
                        alert.Status = AlertStatus.Failed;
                        alert.ErrorMessage = "Email sending failed";
                    }

                    _db.Entry(alert).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Incident added successfully",
                    Data = incident.Id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to add incident",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
        [HttpPut("resolve/{id}")]
        public async Task<IActionResult> ResolveIncident(int id, [FromBody] ResolveIncidentDto dto)
        {
            try
            {
                var incident = await _db.Incidents.FindAsync(id);
                if (incident == null)
                    return NotFound(new ApiResponse<string> { Success = false, Message = "Incident not found" });

                incident.IsResolved = true;
                incident.ResolvedAt = DateTime.UtcNow;
                incident.ResolvedBy = dto.ResolvedBy;
                incident.Notes = dto.Notes;

                _db.Entry(incident).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return Ok(new ApiResponse<string> { Success = true, Message = "Incident resolved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to resolve incident",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // GET: api/incident/summary
        [HttpGet("summary")]
        public IActionResult GetIncidentSummary()
        {
            try
            {
                var total = _db.Incidents.Count();
                var critical = _db.Incidents.Count(i => i.Severity == IncidentSeverity.Critical);
                var resolved = _db.Incidents.Count(i => i.IsResolved);
                var pending = _db.Incidents.Count(i => !i.IsResolved);

                var byType = _db.Incidents
                    .GroupBy(i => i.Type)
                    .Select(g => new IncidentTypeCountDto
                    {
                        Type = g.Key.ToString(),
                        Count = g.Count()
                    })
                    .ToList();

                var byCamera = _db.Incidents
                    .GroupBy(i => i.CameraId)
                    .Select(g => new CameraIncidentCountDto
                    {
                        CameraId = g.Key,
                        CameraName = _db.Cameras.Where(c => c.Id == g.Key).Select(c => c.Name).FirstOrDefault(),
                        Count = g.Count()
                    })
                    .ToList();

                var summary = new IncidentSummaryDto
                {
                    TotalIncidents = total,
                    CriticalIncidents = critical,
                    ResolvedIncidents = resolved,
                    PendingIncidents = pending,
                    IncidentsByType = byType,
                    IncidentsByCamera = byCamera
                };

                return Ok(new ApiResponse<IncidentSummaryDto> { Success = true, Data = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to generate summary",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // GET: api/incident/by-camera/2
        [HttpGet("by-camera/{cameraId}")]
        public IActionResult GetByCamera(int cameraId)
        {
            try
            {
                var incidents = _db.Incidents
                    .Include(i => i.Camera)
                    .Where(i => i.CameraId == cameraId)
                    .Select(i => new IncidentDto
                    {
                        Id = i.Id,
                        CameraId = i.CameraId,
                        CameraName = i.Camera.Name,
                        CameraLocation = i.Camera.Location,
                        Type = i.Type.ToString(),
                        Description = i.Description,
                        ConfidenceScore = i.ConfidenceScore,
                        Severity = i.Severity.ToString(),
                        DetectedAt = i.DetectedAt,
                        IsResolved = i.IsResolved,
                        ResolvedBy = i.ResolvedBy,
                        ResolvedAt = i.ResolvedAt,
                        Notes = i.Notes,
                        VideoClipUrl = i.VideoClipUrl,
                        ImageUrl = i.ImageUrl
                    })
                    .ToList();

                return Ok(new ApiResponse<List<IncidentDto>>
                {
                    Success = true,
                    Message = "Filtered incidents fetched",
                    Data = incidents
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to fetch incidents by camera",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

    }
}
