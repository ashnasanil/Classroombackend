using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Grade
{
    public class GradeSubmissionDto
    {
        [Required]
        [Range(0, 1000)]
        public int Score { get; set; }
        
        public string? TeacherFeedback { get; set; }
    }
}
