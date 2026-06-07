using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Announcement
{
    public class CreateAnnouncementDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? AttachmentUrl { get; set; }
    }
}
