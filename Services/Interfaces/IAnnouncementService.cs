using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Announcement;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementResponseDto> CreateAnnouncementAsync(int classroomId, int userId, CreateAnnouncementDto dto);
        Task<IEnumerable<AnnouncementResponseDto>> GetClassroomAnnouncementsAsync(int classroomId, int userId);
        Task<CommentResponseDto> AddCommentAsync(int announcementId, int userId, CreateCommentDto dto);
        Task DeleteAnnouncementAsync(int announcementId, int userId);
    }
}
