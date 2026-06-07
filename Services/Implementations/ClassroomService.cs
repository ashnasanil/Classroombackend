using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.Classroom;
using GoogleClassroom.API.Models;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Services.Implementations
{
    public class ClassroomService : IClassroomService
    {
        private readonly ApplicationDbContext _context;

        public ClassroomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClassroomResponseDto> CreateClassroomAsync(int userId, CreateClassroomDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var classroom = new Classroom
            {
                Name = dto.ClassName,
                Section = dto.Section,
                Subject = dto.Subject,
                Room = dto.Room,
                Levels = dto.Levels,
                Description = dto.Description,
                CreatedBy = userId,
                ClassCode = GenerateClassCode()
            };

            _context.Classrooms.Add(classroom);
            
            var member = new ClassroomMember
            {
                Classroom = classroom,
                UserId = userId,
                MembershipType = "Teacher"
            };
            _context.ClassroomMembers.Add(member);

            await _context.SaveChangesAsync();

            return MapToResponse(classroom);
        }

        public async Task<ClassroomResponseDto> JoinClassroomAsync(int userId, string classCode)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.ClassCode == classCode && !c.IsArchived);

            if (classroom == null) throw new Exception("Invalid class code or class is archived");

            if (classroom.Members.Any(m => m.UserId == userId))
            {
                throw new Exception("You are already a member of this classroom");
            }

            var member = new ClassroomMember
            {
                ClassroomId = classroom.Id,
                UserId = userId,
                MembershipType = "Student"
            };

            _context.ClassroomMembers.Add(member);
            await _context.SaveChangesAsync();

            return MapToResponse(classroom);
        }

        public async Task<IEnumerable<ClassroomResponseDto>> GetUserClassroomsAsync(int userId, bool isArchived = false)
        {
            var classrooms = await _context.Classrooms
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .Where(c => c.Members.Any(m => m.UserId == userId) && c.IsArchived == isArchived)
                .ToListAsync();

            return classrooms.Select(MapToResponse);
        }

        public async Task<ClassroomResponseDto> GetClassroomByIdAsync(int id, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Creator)
                .Include(c => c.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId))
            {
                throw new Exception("You do not have access to this classroom");
            }

            return MapToResponse(classroom);
        }

        public async Task<ClassroomResponseDto> UpdateClassroomSettingsAsync(int id, int userId, UpdateClassroomSettingsDto dto)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            // Only the teacher/creator can update settings
            if (classroom.CreatedBy != userId)
            {
                throw new Exception("Only the teacher can modify classroom settings");
            }

            // Update Class Details
            classroom.Name = dto.ClassName;
            classroom.Description = dto.Description;
            classroom.Section = dto.Section;
            classroom.Room = dto.Room;
            classroom.Subject = dto.Subject;
            classroom.Levels = dto.Levels;

            // Update General Settings
            classroom.InviteCodesEnabled = dto.InviteCodesEnabled;
            
            // Update Stream Settings
            classroom.StreamPostPermission = dto.StreamPostPermission;
            classroom.ClassworkOnStream = dto.ClassworkOnStream;
            classroom.ShowDeletedItems = dto.ShowDeletedItems;

            // Update Grading Settings
            classroom.ApplyDraftGradeToMissing = dto.ApplyDraftGradeToMissing;
            classroom.MissingAssignmentDefaultGrade = dto.MissingAssignmentDefaultGrade;
            classroom.OverallGradeCalculation = dto.OverallGradeCalculation;
            classroom.ShowOverallGradeToStudents = dto.ShowOverallGradeToStudents;

            classroom.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponse(classroom);
        }

        public async Task InviteUserAsync(int classroomId, int teacherUserId, InviteUserDto dto)
        {
            var classroom = await _context.Classrooms.FindAsync(classroomId);
            if (classroom == null) throw new Exception("Classroom not found");
            if (classroom.CreatedBy != teacherUserId) throw new Exception("Only the teacher can invite users.");

            var userToInvite = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (userToInvite == null) throw new Exception("User not found.");

            var existingMember = await _context.ClassroomMembers.FirstOrDefaultAsync(m => m.ClassroomId == classroomId && m.UserId == userToInvite.Id);
            if (existingMember != null) throw new Exception("User is already in the classroom.");

            var member = new ClassroomMember
            {
                ClassroomId = classroomId,
                UserId = userToInvite.Id,
                MembershipType = dto.Role
            };

            _context.ClassroomMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task<GradebookDto> GetGradebookAsync(int classroomId, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Only the teacher can access the gradebook");

            var students = classroom.Members
                .Where(m => m.MembershipType == "Student")
                .Select(m => new GradebookStudentDto
                {
                    Id = m.UserId,
                    FirstName = m.User?.FirstName ?? "",
                    LastName = m.User?.LastName ?? "",
                    Avatar = m.User?.Avatar
                })
                .ToList();

            var assignments = await _context.Assignments
                .Where(a => a.ClassroomId == classroomId)
                .Select(a => new GradebookAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    Points = a.Points
                })
                .ToListAsync();

            var assignmentIds = assignments.Select(a => a.Id).ToList();

            var submissions = await _context.AssignmentSubmissions
                .Include(s => s.Grade)
                .Where(s => assignmentIds.Contains(s.AssignmentId))
                .Select(s => new GradebookSubmissionDto
                {
                    Id = s.Id,
                    AssignmentId = s.AssignmentId,
                    StudentId = s.UserId,
                    Grade = s.Grade != null ? s.Grade.Score : (int?)null,
                    State = s.IsGraded ? "Graded" : "Turned in"
                })
                .ToListAsync();

            return new GradebookDto
            {
                Students = students,
                Assignments = assignments,
                Submissions = submissions
            };
        }

        public async Task<StudentWorkDto> GetStudentWorkAsync(int classroomId, int studentId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            var studentMember = classroom.Members.FirstOrDefault(m => m.UserId == studentId && m.MembershipType == "Student");
            if (studentMember == null) throw new Exception("Student not found in this classroom");

            var profile = new StudentProfileDto
            {
                Id = studentMember.UserId,
                FirstName = studentMember.User?.FirstName ?? "",
                LastName = studentMember.User?.LastName ?? "",
                Avatar = studentMember.User?.Avatar
            };

            var assignments = await _context.Assignments
                .Where(a => a.ClassroomId == classroomId)
                .ToListAsync();

            var assignmentIds = assignments.Select(a => a.Id).ToList();

            var submissions = await _context.AssignmentSubmissions
                .Include(s => s.Grade)
                .Where(s => s.UserId == studentId && assignmentIds.Contains(s.AssignmentId))
                .ToListAsync();

            var workItems = assignments.Select(a => {
                var submission = submissions.FirstOrDefault(s => s.AssignmentId == a.Id);
                return new StudentWorkItemDto
                {
                    AssignmentId = a.Id,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    MaxPoints = a.Points,
                    SubmissionId = submission?.Id ?? 0,
                    Grade = submission?.Grade?.Score,
                    State = submission != null ? (submission.IsGraded ? "Graded" : "Turned in") : (a.DueDate < DateTime.UtcNow ? "Missing" : "Assigned")
                };
            }).OrderByDescending(w => w.DueDate ?? DateTime.MaxValue).ToList();

            return new StudentWorkDto
            {
                Profile = profile,
                WorkItems = workItems
            };
        }

        public async Task ArchiveClassroomAsync(int id, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Only the teacher can archive the classroom");

            classroom.IsArchived = true;
            await _context.SaveChangesAsync();
        }

        public async Task RestoreClassroomAsync(int id, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Only the teacher can restore the classroom");

            classroom.IsArchived = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassroomAsync(int id, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Only the teacher can delete the classroom");

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();
        }

        public async Task<ClassroomResponseDto> CopyClassroomAsync(int id, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Only a teacher can copy the classroom");

            var newClassroom = new Classroom
            {
                Name = classroom.Name + " (Copy)",
                Section = classroom.Section,
                Subject = classroom.Subject,
                Room = classroom.Room,
                ThemeColor = classroom.ThemeColor,
                CreatedBy = userId,
                ClassCode = GenerateClassCode()
            };

            _context.Classrooms.Add(newClassroom);
            await _context.SaveChangesAsync();

            var member = new ClassroomMember
            {
                ClassroomId = newClassroom.Id,
                UserId = userId,
                MembershipType = "Teacher"
            };
            _context.ClassroomMembers.Add(member);
            await _context.SaveChangesAsync();

            // Refetch with creator
            var finalClass = await _context.Classrooms
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == newClassroom.Id);

            return MapToResponse(finalClass);
        }

        private string GenerateClassCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private ClassroomResponseDto MapToResponse(Classroom classroom)
        {
            return new ClassroomResponseDto
            {
                Id = classroom.Id,
                ClassName = classroom.Name,
                Section = classroom.Section,
                Subject = classroom.Subject,
                Room = classroom.Room,
                Levels = classroom.Levels,
                Description = classroom.Description,
                BannerImage = classroom.ThemeColor,
                ClassCode = classroom.ClassCode,
                TeacherId = classroom.CreatedBy,
                TeacherName = classroom.Creator?.FirstName + " " + classroom.Creator?.LastName,
                IsArchived = classroom.IsArchived,
                CreatedAt = classroom.CreatedAt,
                
                // Settings
                InviteCodesEnabled = classroom.InviteCodesEnabled,
                StreamPostPermission = classroom.StreamPostPermission,
                ClassworkOnStream = classroom.ClassworkOnStream,
                ShowDeletedItems = classroom.ShowDeletedItems,
                ApplyDraftGradeToMissing = classroom.ApplyDraftGradeToMissing,
                MissingAssignmentDefaultGrade = classroom.MissingAssignmentDefaultGrade,
                OverallGradeCalculation = classroom.OverallGradeCalculation,
                ShowOverallGradeToStudents = classroom.ShowOverallGradeToStudents,
                
                // Map Members if loaded
                Members = classroom.Members?.Select(m => new ClassroomMemberDto
                {
                    UserId = m.UserId,
                    FirstName = m.User?.FirstName ?? "",
                    LastName = m.User?.LastName ?? "",
                    ProfilePicture = m.User?.Avatar,
                    MembershipType = m.MembershipType,
                    JoinedAt = m.JoinedAt
                }).ToList() ?? new List<ClassroomMemberDto>()
            };
        }
    }
}
