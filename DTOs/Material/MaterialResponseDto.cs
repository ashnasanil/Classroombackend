using System;
using System.Collections.Generic;
using GoogleClassroom.API.DTOs.Announcement;

namespace GoogleClassroom.API.DTOs.Material
{
    public class MaterialResponseDto
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentResponseDto> Comments { get; set; } = new();
    }
}
