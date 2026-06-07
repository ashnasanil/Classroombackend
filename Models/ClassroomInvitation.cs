using System;

namespace GoogleClassroom.API.Models
{
    public class ClassroomInvitation
    {
        public int Id { get; set; }
        
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;
        
        public string InvitedEmail { get; set; } = string.Empty;
        
        public int InvitedById { get; set; }
        public User InvitedBy { get; set; } = null!;
        
        public string Status { get; set; } = "Pending"; // "Pending", "Accepted", "Declined"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
