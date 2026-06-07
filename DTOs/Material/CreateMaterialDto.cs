using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.DTOs.Material
{
    public class CreateMaterialDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
