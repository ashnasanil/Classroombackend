using System;

namespace GoogleClassroom.API.Models
{
    public class AssignmentSubmission
    {
        public int Id { get; set; }
        
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string? AttachmentUrl { get; set; }
        public string? TextResponse { get; set; }
        
        public bool IsGraded { get; set; } = false;
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public Grade? Grade { get; set; }
        public ICollection<SubmissionComment> Comments { get; set; } = new List<SubmissionComment>();
    }
}
