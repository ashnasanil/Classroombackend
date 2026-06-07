using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Material;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialResponseDto> CreateMaterialAsync(int classroomId, int userId, CreateMaterialDto dto);
        Task<IEnumerable<MaterialResponseDto>> GetClassroomMaterialsAsync(int classroomId, int userId);
        Task<MaterialResponseDto> GetMaterialByIdAsync(int classroomId, int materialId, int userId);
        Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddMaterialCommentAsync(int materialId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto);
    }
}
