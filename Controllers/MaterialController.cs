using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Material;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/classrooms/{classroomId}/materials")]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMaterial(int classroomId, [FromBody] CreateMaterialDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _materialService.CreateMaterialAsync(classroomId, userId, model);
                return CreatedAtAction(nameof(GetMaterials), new { classroomId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterials(int classroomId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _materialService.GetClassroomMaterialsAsync(classroomId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetMaterialById(int classroomId, int materialId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _materialService.GetMaterialByIdAsync(classroomId, materialId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Material not found" || ex.Message == "Classroom not found")
                    return NotFound(new { message = ex.Message });
                    
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{materialId}/comments")]
        public async Task<IActionResult> AddMaterialComment(int classroomId, int materialId, [FromBody] GoogleClassroom.API.DTOs.Announcement.CreateCommentDto model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _materialService.AddMaterialCommentAsync(materialId, userId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
