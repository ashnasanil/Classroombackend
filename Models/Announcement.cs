using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;
        
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;
        
        public string Content { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        
        public bool IsPinned { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AnnouncementComment> Comments { get; set; } = new List<AnnouncementComment>();
    }
}
