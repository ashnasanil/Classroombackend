using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class StudentWorkDto
    {
        public StudentProfileDto Profile { get; set; } = new StudentProfileDto();
        public List<StudentWorkItemDto> WorkItems { get; set; } = new List<StudentWorkItemDto>();
    }

    public class StudentProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
    }

    public class StudentWorkItemDto
    {
        public int AssignmentId { get; set; }
        public string? Title { get; set; }
        public DateTime? DueDate { get; set; }
        public int? MaxPoints { get; set; }
        
        public int SubmissionId { get; set; }
        public int? Grade { get; set; }
        public string? State { get; set; } // "Missing", "Turned in", "Graded", "Assigned"
    }
}
