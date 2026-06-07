using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleClassroom.API.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int ClassroomId { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}
