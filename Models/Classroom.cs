using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Section { get; set; }
        public string? Subject { get; set; }
        public string? Room { get; set; }
        public string? Levels { get; set; }
        public string? Description { get; set; }
        public string? ThemeColor { get; set; }
        public string ClassCode { get; set; } = string.Empty;

        // Settings - General
        public bool InviteCodesEnabled { get; set; } = true;
        
        // Settings - Stream and Classwork
        public string StreamPostPermission { get; set; } = "Students can post and comment";
        public string ClassworkOnStream { get; set; } = "Show condensed notifications";
        public bool ShowDeletedItems { get; set; } = false;

        // Settings - Grading
        public bool ApplyDraftGradeToMissing { get; set; } = true;
        public int MissingAssignmentDefaultGrade { get; set; } = 0;
        public string OverallGradeCalculation { get; set; } = "No overall grade";
        public bool ShowOverallGradeToStudents { get; set; } = false;
        
        public int CreatedBy { get; set; }
        public User Creator { get; set; } = null!;
        
        public bool IsArchived { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ClassroomMember> Members { get; set; } = new List<ClassroomMember>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}
