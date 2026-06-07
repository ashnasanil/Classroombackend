using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Classroom;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;

        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.CreateClassroomAsync(userId, model);
                return CreatedAtAction(nameof(GetClassroom), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> JoinClassroom([FromBody] JoinClassroomDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.JoinClassroomAsync(userId, model.ClassCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserClassrooms([FromQuery] bool isArchived = false)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.GetUserClassroomsAsync(userId, isArchived);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroom(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.GetClassroomByIdAsync(id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/settings")]
        [Authorize]
        public async Task<IActionResult> UpdateSettings(int id, [FromBody] UpdateClassroomSettingsDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.UpdateClassroomSettingsAsync(id, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found") || ex.Message.Contains("Only the teacher can modify"))
                {
                    return Forbid(); // Or NotFound depending on preference, but Forbid fits permissions
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/invite")]
        [Authorize]
        public async Task<IActionResult> InviteUser(int id, [FromBody] InviteUserDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _classroomService.InviteUserAsync(id, userId, model);
                return Ok(new { message = "User invited successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/gradebook")]
        [Authorize]
        public async Task<IActionResult> GetGradebook(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var gradebook = await _classroomService.GetGradebookAsync(id, userId);
                return Ok(gradebook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/student-work")]
        [Authorize]
        public async Task<IActionResult> GetStudentWork(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var studentWork = await _classroomService.GetStudentWorkAsync(id, userId);
                return Ok(studentWork);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/archive")]
        [Authorize]
        public async Task<IActionResult> ArchiveClassroom(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _classroomService.ArchiveClassroomAsync(id, userId);
                return Ok(new { message = "Classroom archived successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/restore")]
        [Authorize]
        public async Task<IActionResult> RestoreClassroom(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _classroomService.RestoreClassroomAsync(id, userId);
                return Ok(new { message = "Classroom restored successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _classroomService.DeleteClassroomAsync(id, userId);
                return Ok(new { message = "Classroom deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/copy")]
        [Authorize]
        public async Task<IActionResult> CopyClassroom(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _classroomService.CopyClassroomAsync(id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
