using System;

namespace GoogleClassroom.API.Models
{
    public class Grade
    {
        public int Id { get; set; }
        
        public int AssignmentSubmissionId { get; set; }
        public AssignmentSubmission AssignmentSubmission { get; set; } = null!;
        
        public int Score { get; set; }
        public string? TeacherFeedback { get; set; }
        
        public int GraderId { get; set; }
        public User Grader { get; set; } = null!;
        
        public DateTime GradedAt { get; set; } = DateTime.UtcNow;
    }
}
