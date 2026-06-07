using System;

namespace GoogleClassroom.API.Models
{
    public class MaterialComment
    {
        public int Id { get; set; }
        
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
