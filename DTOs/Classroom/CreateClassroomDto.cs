using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class CreateClassroomDto
    {
        [Required]
        [MaxLength(100)]
        public string ClassName { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? Section { get; set; }
        
        [MaxLength(100)]
        public string? Subject { get; set; }
        
        [MaxLength(100)]
        public string? Room { get; set; }
        
        public string? Levels { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
