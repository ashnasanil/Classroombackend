using System;

namespace GoogleClassroom.API.Models
{
    public class ClassroomMember
    {
        public int Id { get; set; }
        
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string MembershipType { get; set; } = "Student"; // "Teacher" or "Student"
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
