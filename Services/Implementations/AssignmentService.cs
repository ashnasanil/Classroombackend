using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.Assignment;
using GoogleClassroom.API.DTOs.Grade;
using GoogleClassroom.API.DTOs.Submission;
using GoogleClassroom.API.Models;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AssignmentResponseDto> CreateAssignmentAsync(int classroomId, int userId, CreateAssignmentDto dto)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null || !classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Classroom not found or you are not a teacher");

            var assignment = new Assignment
            {
                ClassroomId = classroomId,
                AuthorId = userId,
                Title = dto.Title,
                Description = dto.Description,
                AttachmentUrl = dto.AttachmentUrl,
                Points = dto.Points,
                DueDate = dto.DueDate
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            await _context.Entry(assignment).Reference(a => a.Author).LoadAsync();

            return MapToResponse(assignment);
        }

        public async Task<IEnumerable<AssignmentResponseDto>> GetClassroomAssignmentsAsync(int classroomId, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You do not have access to this classroom");

            var assignments = await _context.Assignments
                .Include(a => a.Author)
                .Include(a => a.Submissions)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .Include(a => a.Classroom)
                    .ThenInclude(c => c.Members)
                .Where(a => a.ClassroomId == classroomId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assignments.Select(MapToResponse);
        }

        public async Task<IEnumerable<AssignmentResponseDto>> GetUserAssignmentsAsync(int userId)
        {
            var classroomIds = await _context.ClassroomMembers
                .Where(m => m.UserId == userId)
                .Select(m => m.ClassroomId)
                .ToListAsync();

            var assignments = await _context.Assignments
                .Include(a => a.Author)
                .Include(a => a.Classroom)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .Include(a => a.Submissions.Where(s => s.UserId == userId))
                .Where(a => classroomIds.Contains(a.ClassroomId))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assignments.Select(a => {
                var response = MapToResponse(a);
                var submission = a.Submissions?.FirstOrDefault();
                response.IsSubmitted = submission?.SubmittedAt != null;
                response.SubmittedAt = submission?.SubmittedAt;
                response.ClassroomName = a.Classroom?.Name;
                return response;
            });
        }

        public async Task<AssignmentResponseDto> UpdateAssignmentAsync(int assignmentId, int userId, CreateAssignmentDto dto)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Author)
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("You are not authorized to update this assignment");

            assignment.Title = dto.Title;
            assignment.Description = dto.Description;
            assignment.AttachmentUrl = dto.AttachmentUrl;
            assignment.Points = dto.Points;
            assignment.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();
            return MapToResponse(assignment);
        }

        public async Task DeleteAssignmentAsync(int assignmentId, int userId)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("You are not authorized to delete this assignment");

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<SubmissionResponseDto> SubmitAssignmentAsync(int assignmentId, int userId, SubmitAssignmentDto dto)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                    .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Student"))
                throw new Exception("You are not a student in this classroom");

            var submission = await _context.AssignmentSubmissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.UserId == userId);

            if (submission != null)
            {
                submission.AttachmentUrl = dto.AttachmentUrl ?? submission.AttachmentUrl;
                submission.TextResponse = dto.TextResponse ?? submission.TextResponse;
                submission.SubmittedAt = DateTime.UtcNow;
            }
            else
            {
                submission = new AssignmentSubmission
                {
                    AssignmentId = assignmentId,
                    UserId = userId,
                    AttachmentUrl = dto.AttachmentUrl,
                    TextResponse = dto.TextResponse
                };
                _context.AssignmentSubmissions.Add(submission);
            }

            await _context.SaveChangesAsync();
            await _context.Entry(submission).Reference(s => s.User).LoadAsync();
            await _context.Entry(submission).Reference(s => s.Grade).LoadAsync();

            return MapSubmissionToResponse(submission);
        }

        public async Task UnsubmitAssignmentAsync(int assignmentId, int userId)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Student"))
                throw new Exception("You are not a student in this classroom");

            var submission = await _context.AssignmentSubmissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.UserId == userId);

            if (submission != null)
            {
                _context.AssignmentSubmissions.Remove(submission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SubmissionResponseDto>> GetAssignmentSubmissionsAsync(int assignmentId, int userId)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
                throw new Exception("Assignment not found");

            var member = assignment.Classroom.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null)
                throw new Exception("You are not a member of this classroom");

            var query = _context.AssignmentSubmissions
                .Include(s => s.User)
                .Include(s => s.Grade)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .Where(s => s.AssignmentId == assignmentId);

            // If the user is a student, only return their own submission
            if (member.MembershipType == "Student")
            {
                query = query.Where(s => s.UserId == userId);
            }
            // If the user is a teacher, the query remains unchanged (returns all submissions)

            var submissions = await query.ToListAsync();
            return submissions.Select(MapSubmissionToResponse);
        }

        public async Task<SubmissionResponseDto> GradeSubmissionAsync(int submissionId, int userId, GradeSubmissionDto dto)
        {
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .ThenInclude(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .Include(s => s.Grade)
                .Include(s => s.User)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null) throw new Exception("Submission not found");

            if (!submission.Assignment.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("You are not a teacher in this classroom");

            if (submission.Grade != null)
            {
                submission.Grade.Score = dto.Score;
                submission.Grade.TeacherFeedback = dto.TeacherFeedback;
                submission.Grade.GradedAt = DateTime.UtcNow;
            }
            else
            {
                var grade = new Grade
                {
                    AssignmentSubmissionId = submissionId,
                    GraderId = userId,
                    Score = dto.Score,
                    TeacherFeedback = dto.TeacherFeedback
                };
                _context.Grades.Add(grade);
                submission.IsGraded = true;
                submission.Grade = grade;
            }

            await _context.SaveChangesAsync();

            return MapSubmissionToResponse(submission);
        }

        public async Task<SubmissionResponseDto> GradeStudentAsync(int assignmentId, int studentId, int teacherId, GradeSubmissionDto dto)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == teacherId && m.MembershipType == "Teacher"))
                throw new Exception("You are not a teacher in this classroom");

            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Grade)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.UserId == studentId);

            if (submission == null)
            {
                submission = new AssignmentSubmission
                {
                    AssignmentId = assignmentId,
                    UserId = studentId,
                    SubmittedAt = DateTime.UtcNow,
                    AttachmentUrl = null,
                    TextResponse = null,
                    IsGraded = true
                };
                _context.AssignmentSubmissions.Add(submission);
                
                var grade = new Grade
                {
                    AssignmentSubmission = submission,
                    GraderId = teacherId,
                    Score = dto.Score,
                    TeacherFeedback = dto.TeacherFeedback
                };
                _context.Grades.Add(grade);
                submission.Grade = grade;
            }
            else
            {
                if (submission.Grade != null)
                {
                    submission.Grade.Score = dto.Score;
                    submission.Grade.TeacherFeedback = dto.TeacherFeedback;
                    submission.Grade.GradedAt = DateTime.UtcNow;
                }
                else
                {
                    var grade = new Grade
                    {
                        AssignmentSubmissionId = submission.Id,
                        GraderId = teacherId,
                        Score = dto.Score,
                        TeacherFeedback = dto.TeacherFeedback
                    };
                    _context.Grades.Add(grade);
                    submission.IsGraded = true;
                    submission.Grade = grade;
                }
            }

            await _context.SaveChangesAsync();

            // Refetch to get related entities properly loaded for MapToResponse
            var completeSubmission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Grade)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == submission.Id);

            return MapSubmissionToResponse(completeSubmission);
        }

        private AssignmentResponseDto MapToResponse(Assignment assignment)
        {
            return new AssignmentResponseDto
            {
                Id = assignment.Id,
                ClassroomId = assignment.ClassroomId,
                TeacherId = assignment.AuthorId, // DTO alias for now
                TeacherName = assignment.Author?.FirstName + " " + assignment.Author?.LastName,
                Title = assignment.Title,
                Description = assignment.Description,
                AttachmentUrl = assignment.AttachmentUrl,
                Points = assignment.Points,
                DueDate = assignment.DueDate.HasValue ? DateTime.SpecifyKind(assignment.DueDate.Value, DateTimeKind.Utc) : null,
                CreatedAt = DateTime.SpecifyKind(assignment.CreatedAt, DateTimeKind.Utc),
                TurnedInCount = assignment.Submissions?.Count() ?? 0,
                AssignedCount = assignment.Classroom?.Members?.Count(m => m.MembershipType == "Student") ?? 0,
                Comments = assignment.Comments?.Select(c => new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User?.FirstName + " " + c.User?.LastName,
                    Comment = c.Content,
                    CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)
                }).OrderBy(c => c.CreatedAt).ToList() ?? new List<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto>()
            };
        }

        private SubmissionResponseDto MapSubmissionToResponse(AssignmentSubmission submission)
        {
            return new SubmissionResponseDto
            {
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                StudentId = submission.UserId, // DTO alias
                StudentName = submission.User?.FirstName + " " + submission.User?.LastName,
                AttachmentUrl = submission.AttachmentUrl,
                TextResponse = submission.TextResponse,
                IsGraded = submission.IsGraded,
                Score = submission.Grade?.Score,
                TeacherFeedback = submission.Grade?.TeacherFeedback,
                SubmittedAt = DateTime.SpecifyKind(submission.SubmittedAt, DateTimeKind.Utc),
                Comments = submission.Comments?.Select(c => new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User?.FirstName + " " + c.User?.LastName,
                    Comment = c.Content,
                    CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)
                }).OrderBy(c => c.CreatedAt).ToList() ?? new List<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto>()
            };
        }

        public async Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddAssignmentCommentAsync(int assignmentId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Classroom)
                    .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) throw new Exception("Assignment not found");

            if (!assignment.Classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You are not a member of this classroom");

            var comment = new AssignmentComment
            {
                AssignmentId = assignmentId,
                UserId = userId,
                Content = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.AssignmentComments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Entry(comment).Reference(c => c.User).LoadAsync();

            return new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User.FirstName + " " + comment.User.LastName,
                Comment = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddSubmissionCommentAsync(int submissionId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto)
        {
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Classroom)
                        .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null) throw new Exception("Submission not found");

            var member = submission.Assignment.Classroom.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null)
                throw new Exception("You are not a member of this classroom");

            // Only the student who submitted or a teacher can comment
            if (member.MembershipType == "Student" && submission.UserId != userId)
                throw new Exception("You are not authorized to comment on this submission");

            var comment = new SubmissionComment
            {
                AssignmentSubmissionId = submissionId,
                UserId = userId,
                Content = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.SubmissionComments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Entry(comment).Reference(c => c.User).LoadAsync();

            return new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User.FirstName + " " + comment.User.LastName,
                Comment = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
