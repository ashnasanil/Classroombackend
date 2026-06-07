using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;
        
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;
        
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? Points { get; set; } = 100;
        
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
        public ICollection<AssignmentComment> Comments { get; set; } = new List<AssignmentComment>();
    }
}
