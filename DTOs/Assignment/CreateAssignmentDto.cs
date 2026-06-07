using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Assignment
{
    public class CreateAssignmentDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
        
        public int? Points { get; set; } = 100;
        
        public DateTime? DueDate { get; set; }
    }
}
