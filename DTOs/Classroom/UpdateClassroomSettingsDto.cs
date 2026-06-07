using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class UpdateClassroomSettingsDto
    {
        // Class Details
        [Required]
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Section { get; set; }
        public string? Room { get; set; }
        public string? Subject { get; set; }
        public string? Levels { get; set; }

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
