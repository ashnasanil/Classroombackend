using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Announcement
{
    public class CreateCommentDto
    {
        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
