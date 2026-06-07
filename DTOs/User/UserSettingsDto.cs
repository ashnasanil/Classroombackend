using System;

namespace GoogleClassroom.API.DTOs.User
{
    public class UserSettingsDto
    {
        // Email
        public bool AllowEmailNotifications { get; set; }
        
        // Comments
        public bool CommentsOnYourPosts { get; set; }
        public bool CommentsThatMentionYou { get; set; }
        public bool PrivateCommentsOnWork { get; set; }
        
        // Classes you're enrolled in
        public bool WorkAndPostsFromTeachers { get; set; }
        public bool ReturnedWorkAndGrades { get; set; }
        public bool InvitationsToJoinClasses { get; set; }
        public bool DueDateReminders { get; set; }
        
        // Classes you teach
        public bool LateSubmissions { get; set; }
        public bool Resubmissions { get; set; }
        public bool InvitationsToCoteach { get; set; }
        public bool ScheduledPostPublished { get; set; }
        
        // Homepage
        public bool ShowDisplayNameOnHomepage { get; set; }
    }
}
