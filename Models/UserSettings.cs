using System;

namespace GoogleClassroom.API.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        
        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Email
        public bool AllowEmailNotifications { get; set; } = true;
        
        // Comments
        public bool CommentsOnYourPosts { get; set; } = true;
        public bool CommentsThatMentionYou { get; set; } = true;
        public bool PrivateCommentsOnWork { get; set; } = true;
        
        // Classes you're enrolled in
        public bool WorkAndPostsFromTeachers { get; set; } = true;
        public bool ReturnedWorkAndGrades { get; set; } = true;
        public bool InvitationsToJoinClasses { get; set; } = true;
        public bool DueDateReminders { get; set; } = true;
        
        // Classes you teach
        public bool LateSubmissions { get; set; } = true;
        public bool Resubmissions { get; set; } = true;
        public bool InvitationsToCoteach { get; set; } = true;
        public bool ScheduledPostPublished { get; set; } = true;
        
        // Homepage
        public bool ShowDisplayNameOnHomepage { get; set; } = true;
    }
}
