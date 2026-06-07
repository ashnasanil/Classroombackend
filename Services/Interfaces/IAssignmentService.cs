using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Assignment;
using GoogleClassroom.API.DTOs.Grade;
using GoogleClassroom.API.DTOs.Submission;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentResponseDto> CreateAssignmentAsync(int classroomId, int userId, CreateAssignmentDto dto);
        Task<IEnumerable<AssignmentResponseDto>> GetClassroomAssignmentsAsync(int classroomId, int userId);
        Task<IEnumerable<AssignmentResponseDto>> GetUserAssignmentsAsync(int userId);
        Task<AssignmentResponseDto> UpdateAssignmentAsync(int assignmentId, int userId, CreateAssignmentDto dto);
        Task DeleteAssignmentAsync(int assignmentId, int userId);
        Task<SubmissionResponseDto> SubmitAssignmentAsync(int assignmentId, int userId, SubmitAssignmentDto dto);
        Task UnsubmitAssignmentAsync(int assignmentId, int userId);
        Task<IEnumerable<SubmissionResponseDto>> GetAssignmentSubmissionsAsync(int assignmentId, int userId);
        Task<SubmissionResponseDto> GradeSubmissionAsync(int submissionId, int userId, GradeSubmissionDto dto);
        Task<SubmissionResponseDto> GradeStudentAsync(int assignmentId, int studentId, int teacherId, GradeSubmissionDto dto);
        Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddAssignmentCommentAsync(int assignmentId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto);
        Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddSubmissionCommentAsync(int submissionId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto);
    }
}
