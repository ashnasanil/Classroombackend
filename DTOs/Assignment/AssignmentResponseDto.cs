using System;
using System.Collections.Generic;
using GoogleClassroom.API.DTOs.Announcement;

namespace GoogleClassroom.API.DTOs.Assignment
{
    public class AssignmentResponseDto
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? Points { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string? ClassroomName { get; set; }
        public int TurnedInCount { get; set; }
        public int AssignedCount { get; set; }
        public List<CommentResponseDto> Comments { get; set; } = new();
    }
}
