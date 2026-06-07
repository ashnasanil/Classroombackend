using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Classroom;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IClassroomService
    {
        Task<ClassroomResponseDto> CreateClassroomAsync(int userId, CreateClassroomDto dto);
        Task<ClassroomResponseDto> JoinClassroomAsync(int userId, string classCode);
        Task<IEnumerable<ClassroomResponseDto>> GetUserClassroomsAsync(int userId, bool isArchived = false);
        Task<ClassroomResponseDto> GetClassroomByIdAsync(int id, int userId);
        Task<ClassroomResponseDto> UpdateClassroomSettingsAsync(int id, int userId, UpdateClassroomSettingsDto dto);
        Task<GradebookDto> GetGradebookAsync(int classroomId, int userId);
        Task<StudentWorkDto> GetStudentWorkAsync(int classroomId, int studentId);
        Task ArchiveClassroomAsync(int id, int userId);
        Task RestoreClassroomAsync(int id, int userId);
        Task DeleteClassroomAsync(int id, int userId);
        Task<ClassroomResponseDto> CopyClassroomAsync(int id, int userId);
        Task InviteUserAsync(int classroomId, int teacherUserId, InviteUserDto dto);
    }
}
