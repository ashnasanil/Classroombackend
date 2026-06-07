using System;
using System.Collections.Generic;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class GradebookDto
    {
        public List<GradebookStudentDto> Students { get; set; } = new List<GradebookStudentDto>();
        public List<GradebookAssignmentDto> Assignments { get; set; } = new List<GradebookAssignmentDto>();
        public List<GradebookSubmissionDto> Submissions { get; set; } = new List<GradebookSubmissionDto>();
    }

    public class GradebookStudentDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
    }

    public class GradebookAssignmentDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Points { get; set; }
    }

    public class GradebookSubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public int? Grade { get; set; }
        public string? State { get; set; } // "Missing", "Turned in", "Graded", "Assigned"
    }
}
