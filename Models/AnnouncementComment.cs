using System;

namespace GoogleClassroom.API.Models
{
    public class AnnouncementComment
    {
        public int Id { get; set; }
        
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Comment { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
