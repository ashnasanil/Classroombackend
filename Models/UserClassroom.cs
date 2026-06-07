using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.API.Models
{
    public class UserClassroom
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty; // "Teacher" or "Student"
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
