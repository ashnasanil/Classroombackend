using System;
using System.Collections.Generic;
using GoogleClassroom.API.DTOs.Announcement;

namespace GoogleClassroom.API.DTOs.Submission
{
    public class SubmissionResponseDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public string? TextResponse { get; set; }
        public bool IsGraded { get; set; }
        public int? Score { get; set; }
        public string? TeacherFeedback { get; set; }
        public DateTime SubmittedAt { get; set; }
        public List<CommentResponseDto> Comments { get; set; } = new();
    }
}
