using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.DTOs.Announcement
{
    public class AnnouncementResponseDto
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public bool IsPinned { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();
    }
}
