using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVision_AI.API.Data;
using SafeVisionAI.Core.Entities;
using SafeVisionAI.Infrastructure.DTOs;

namespace SafeVision_AI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly SafeVisionDbContext _context;

        public CamerasController(SafeVisionDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCamera([FromBody] CreateCameraDto dto)
        {
            if (dto == null)
                return BadRequest(new { status = 400, message = "Invalid camera data." });

            try
            {
                var camera = new Camera
                {
                    Name = dto.Name,
                    StreamUrl = dto.StreamUrl,
                    Location = dto.Location,
                    FrameRate = dto.FrameRate,
                    AnalysisInterval = dto.AnalysisInterval,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    LastSeen = DateTime.UtcNow
                };
                _context.Cameras.Add(camera);
                await _context.SaveChangesAsync();
                return Ok(new { status = 200, message = "Camera added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCameras()
        {
            try
            {
                var cameras = await _context.Cameras
                    .Select(c => new CameraDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StreamUrl = c.StreamUrl,
                        Location = c.Location,
                        IsActive = c.IsActive,
                        LastSeen = c.LastSeen,
                        CreatedAt = c.CreatedAt,
                        TotalIncidents = c.Incidents.Count,
                        TodayIncidents = c.Incidents.Count(i => i.DetectedAt.Date == DateTime.UtcNow.Date)
                    }).ToListAsync();

                return Ok(new { status = 200, message = "Success", cameras });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCameraById(int id)
        {
            try
            {
                var camera = await _context.Cameras
                    .Where(c => c.Id == id)
                    .Select(c => new CameraDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StreamUrl = c.StreamUrl,
                        Location = c.Location,
                        IsActive = c.IsActive,
                        LastSeen = c.LastSeen,
                        CreatedAt = c.CreatedAt,
                        TotalIncidents = c.Incidents.Count,
                        TodayIncidents = c.Incidents.Count(i => i.DetectedAt.Date == DateTime.UtcNow.Date)
                    }).FirstOrDefaultAsync();

                if (camera == null)
                    return NotFound(new { status = 404, message = "Camera not found." });

                return Ok(new { status = 200, message = "Success", camera });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCamera(int id, [FromBody] UpdateCameraDto dto)
        {
            try
            {
                var camera = await _context.Cameras.FindAsync(id);
                if (camera == null)
                    return NotFound(new { status = 404, message = "Camera not found." });

                camera.Name = dto.Name ?? camera.Name;
                camera.StreamUrl = dto.StreamUrl ?? camera.StreamUrl;
                camera.Location = dto.Location ?? camera.Location;
                camera.IsActive = dto.IsActive ?? camera.IsActive;
                camera.FrameRate = dto.FrameRate ?? camera.FrameRate;
                camera.AnalysisInterval = dto.AnalysisInterval ?? camera.AnalysisInterval;
                camera.LastSeen = DateTime.UtcNow;

                _context.Entry(camera).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = 200, message = "Camera updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal Server Error", error = ex.Message });
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCamera(int id)
        {
            try
            {
                var camera = await _context.Cameras.FindAsync(id);
                if (camera == null)
                    return NotFound(new { status = 404, message = "Camera not found." });

                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();

                return Ok(new { status = 200, message = "Camera deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}
