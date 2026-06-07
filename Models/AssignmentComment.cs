using System;

namespace GoogleClassroom.API.Models
{
    public class AssignmentComment
    {
        public int Id { get; set; }
        
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
