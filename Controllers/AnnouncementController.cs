using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Announcement;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/classrooms/{classroomId}/announcements")]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAnnouncement(int classroomId, [FromBody] CreateAnnouncementDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _announcementService.CreateAnnouncementAsync(classroomId, userId, model);
                return CreatedAtAction(nameof(GetAnnouncements), new { classroomId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements(int classroomId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _announcementService.GetClassroomAnnouncementsAsync(classroomId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{announcementId}/comments")]
        public async Task<IActionResult> AddComment(int announcementId, [FromBody] CreateCommentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _announcementService.AddCommentAsync(announcementId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{announcementId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAnnouncement(int announcementId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _announcementService.DeleteAnnouncementAsync(announcementId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
