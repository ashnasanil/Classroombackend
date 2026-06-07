using System;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class ClassroomResponseDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Section { get; set; }
        public string? Subject { get; set; }
        public string? Room { get; set; }
        public string? Levels { get; set; }
        public string? Description { get; set; }
        public string? BannerImage { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }

        public System.Collections.Generic.List<ClassroomMemberDto> Members { get; set; } = new();

        // Settings - General
        public bool InviteCodesEnabled { get; set; }
        
        // Settings - Stream and Classwork
        public string StreamPostPermission { get; set; } = string.Empty;
        public string ClassworkOnStream { get; set; } = string.Empty;
        public bool ShowDeletedItems { get; set; }

        // Settings - Grading
        public bool ApplyDraftGradeToMissing { get; set; }
        public int MissingAssignmentDefaultGrade { get; set; }
        public string OverallGradeCalculation { get; set; } = string.Empty;
        public bool ShowOverallGradeToStudents { get; set; }
    }
}
