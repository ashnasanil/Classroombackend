using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class JoinClassroomDto
    {
        [Required]
        public string ClassCode { get; set; } = string.Empty;
    }
}
