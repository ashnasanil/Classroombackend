using System;

namespace GoogleClassroom.API.Models
{
    public class Material
    {
        public int Id { get; set; }
        
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;
        
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;
        
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public System.Collections.Generic.ICollection<MaterialComment> Comments { get; set; } = new System.Collections.Generic.List<MaterialComment>();
    }
}
