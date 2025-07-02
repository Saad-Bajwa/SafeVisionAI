using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVision_AI.API.Data;
using SafeVision_AI.API.Interfaces;
using SafeVision_AI.API.Services;
using SafeVisionAI.Core.Entities;
using SafeVisionAI.Core.Enums;
using SafeVisionAI.Infrastructure.DTOs;

namespace SafeVision_AI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly SafeVisionDbContext _db;
        private readonly IEmailNotificationService _emailNotificationService;

        public AlertsController(SafeVisionDbContext db, IEmailNotificationService emailNotificationService)
        {
            _db = db;
            _emailNotificationService = emailNotificationService;
        }

        // GET: api/incidentalerts/all
        [HttpGet("all")]
        public IActionResult GetAllAlerts()
        {
            try
            {
                var alerts = _db.IncidentAlerts
                    .Include(a => a.Incident)
                    .Select(a => new AlertDto
                    {
                        Id = a.Id,
                        IncidentId = a.IncidentId,
                        RecipientEmail = a.RecipientEmail,
                        RecipientPhone = a.RecipientPhone ?? String.Empty,
                        Status = a.Status.ToString(),
                        CreatedAt = a.CreatedAt,
                        SentAt = a.SentAt,
                        AcknowledgedAt = a.AcknowledgedAt,
                        ErrorMessage = a.ErrorMessage ?? String.Empty
                    })
                    .ToList();

                return Ok(new ApiResponse<List<AlertDto>>
                {
                    Success = true,
                    Message = "Alerts fetched successfully",
                    Data = alerts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to fetch alerts",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // GET: api/incidentalerts/{id}
        [HttpGet("{id}")]
        public IActionResult GetAlertById(int id)
        {
            try
            {
                var alert = _db.IncidentAlerts
                    .Include(a => a.Incident)
                    .Where(a => a.Id == id)
                    .Select(a => new AlertDto
                    {
                        Id = a.Id,
                        IncidentId = a.IncidentId,
                        RecipientEmail = a.RecipientEmail,
                        RecipientPhone = a.RecipientPhone ?? String.Empty,
                        Status = a.Status.ToString(),
                        CreatedAt = a.CreatedAt,
                        SentAt = a.SentAt,
                        AcknowledgedAt = a.AcknowledgedAt,
                        ErrorMessage = a.ErrorMessage ?? String.Empty
                    })
                    .FirstOrDefault();

                if (alert == null)
                    return NotFound(new ApiResponse<string> { Success = false, Message = "Alert not found" });

                return Ok(new ApiResponse<AlertDto> { Success = true, Data = alert });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to fetch alert",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // POST: api/incidentalerts/add
        [HttpPost("add")]
        public async Task<IActionResult> AddAlert([FromBody] AlertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RecipientEmail))
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Invalid data" });

            try
            {
                var incident = await _db.Incidents
                        .Include(i => i.Camera)
                        .FirstOrDefaultAsync(i => i.Id == dto.IncidentId);

                if (incident == null)
                    return BadRequest(new ApiResponse<string> { Success = false, Message = "Incident not found" });

                var alert = new IncidentAlert
                {
                    IncidentId = dto.IncidentId,
                    RecipientEmail = dto.RecipientEmail,
                    RecipientPhone = dto.RecipientPhone,
                    Status = AlertStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _db.IncidentAlerts.Add(alert);
                await _db.SaveChangesAsync();

                var emailBody = EmailBodyService.GenerateAlertEmailBody(incident, alert);
                var subject = $"[SafeVision AI] {incident.Type} Alert - {incident.Camera?.Location ?? "Unknown Location"}";


                var emailSent = await _emailNotificationService.SendEmail(alert.RecipientEmail, subject, emailBody);

                if (emailSent)
                {
                    alert.Status = AlertStatus.Sent;
                    alert.SentAt = DateTime.UtcNow;
                    _db.Entry(alert).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Alert added successfully",
                    Data = alert.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to add alert",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // PUT: api/incidentalerts/update-status/{id}
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateAlertStatus(int id, [FromBody] UpdateAlertStatusDto dto)
        {
            try
            {
                var alert = await _db.IncidentAlerts.FindAsync(id);
                if (alert == null)
                    return NotFound(new ApiResponse<string> { Success = false, Message = "Alert not found" });

                if (!Enum.TryParse<AlertStatus>(dto.Status, out var status))
                    return BadRequest(new ApiResponse<string> { Success = false, Message = "Invalid alert status" });

                alert.Status = status;

                if (status == AlertStatus.Sent)
                    alert.SentAt = DateTime.UtcNow;
                if (status == AlertStatus.Acknowledged)
                    alert.AcknowledgedAt = DateTime.UtcNow;

                alert.ErrorMessage = dto.ErrorMessage;

                _db.Entry(alert).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return Ok(new ApiResponse<string> { Success = true, Message = "Alert status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to update alert",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // DELETE: api/incidentalerts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            try
            {
                var alert = await _db.IncidentAlerts.FindAsync(id);
                if (alert == null)
                    return NotFound(new ApiResponse<string> { Success = false, Message = "Alert not found" });

                _db.IncidentAlerts.Remove(alert);
                await _db.SaveChangesAsync();

                return Ok(new ApiResponse<string> { Success = true, Message = "Alert deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to delete alert",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

    }
}
