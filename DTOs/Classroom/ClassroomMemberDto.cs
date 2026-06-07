using System;

namespace GoogleClassroom.API.DTOs.Classroom
{
    public class ClassroomMemberDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string MembershipType { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
