using System;

namespace GoogleClassroom.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? RecoveryEmail { get; set; }
        public string? Avatar { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<ClassroomMember> ClassroomMemberships { get; set; } = new List<ClassroomMember>();
        public ICollection<Classroom> CreatedClassrooms { get; set; } = new List<Classroom>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<AnnouncementComment> Comments { get; set; } = new List<AnnouncementComment>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        
        public UserSettings? Settings { get; set; }
    }
}
