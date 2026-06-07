using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Assignment;
using GoogleClassroom.API.DTOs.Grade;
using GoogleClassroom.API.DTOs.Submission;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/classrooms/{classroomId}/assignments")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAssignment(int classroomId, [FromBody] CreateAssignmentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.CreateAssignmentAsync(classroomId, userId, model);
                return CreatedAtAction(nameof(GetAssignments), new { classroomId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignments(int classroomId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.GetClassroomAssignmentsAsync(classroomId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/api/users/me/assignments")]
        public async Task<IActionResult> GetUserAssignments()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.GetUserAssignmentsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{assignmentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAssignment(int classroomId, int assignmentId, [FromBody] CreateAssignmentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.UpdateAssignmentAsync(assignmentId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{assignmentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAssignment(int classroomId, int assignmentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _assignmentService.DeleteAssignmentAsync(assignmentId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assignmentId}/submit")]
        [Authorize]
        public async Task<IActionResult> SubmitAssignment(int classroomId, int assignmentId, [FromBody] SubmitAssignmentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.SubmitAssignmentAsync(assignmentId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{assignmentId}/submit")]
        [Authorize]
        public async Task<IActionResult> UnsubmitAssignment(int classroomId, int assignmentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _assignmentService.UnsubmitAssignmentAsync(assignmentId, userId);
                return Ok(new { message = "Unsubmitted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{assignmentId}/submissions")]
        [Authorize]
        public async Task<IActionResult> GetSubmissions(int classroomId, int assignmentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.GetAssignmentSubmissionsAsync(assignmentId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assignmentId}/submissions/{submissionId}/grade")]
        [Authorize]
        public async Task<IActionResult> GradeSubmission(int classroomId, int assignmentId, int submissionId, [FromBody] GradeSubmissionDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.GradeSubmissionAsync(submissionId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("{assignmentId}/students/{studentId}/grade")]
        [Authorize]
        public async Task<IActionResult> GradeStudent(int classroomId, int assignmentId, int studentId, [FromBody] GradeSubmissionDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.GradeStudentAsync(assignmentId, studentId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assignmentId}/comments")]
        public async Task<IActionResult> AddAssignmentComment(int classroomId, int assignmentId, [FromBody] GoogleClassroom.API.DTOs.Announcement.CreateCommentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.AddAssignmentCommentAsync(assignmentId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("submissions/{submissionId}/comments")]
        public async Task<IActionResult> AddSubmissionComment(int classroomId, int submissionId, [FromBody] GoogleClassroom.API.DTOs.Announcement.CreateCommentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _assignmentService.AddSubmissionCommentAsync(submissionId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
