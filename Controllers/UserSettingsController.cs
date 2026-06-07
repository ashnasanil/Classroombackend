using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.User;
using GoogleClassroom.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Controllers
{
    [ApiController]
    [Route("api/users/me/settings")]
    [Authorize]
    public class UserSettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
                
                if (settings == null)
                {
                    // Create default settings if they don't exist
                    settings = new UserSettings { UserId = userId };
                    _context.UserSettings.Add(settings);
                    await _context.SaveChangesAsync();
                }

                var dto = new UserSettingsDto
                {
                    AllowEmailNotifications = settings.AllowEmailNotifications,
                    CommentsOnYourPosts = settings.CommentsOnYourPosts,
                    CommentsThatMentionYou = settings.CommentsThatMentionYou,
                    PrivateCommentsOnWork = settings.PrivateCommentsOnWork,
                    WorkAndPostsFromTeachers = settings.WorkAndPostsFromTeachers,
                    ReturnedWorkAndGrades = settings.ReturnedWorkAndGrades,
                    InvitationsToJoinClasses = settings.InvitationsToJoinClasses,
                    DueDateReminders = settings.DueDateReminders,
                    LateSubmissions = settings.LateSubmissions,
                    Resubmissions = settings.Resubmissions,
                    InvitationsToCoteach = settings.InvitationsToCoteach,
                    ScheduledPostPublished = settings.ScheduledPostPublished,
                    ShowDisplayNameOnHomepage = settings.ShowDisplayNameOnHomepage
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UserSettingsDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var settings = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
                
                if (settings == null)
                {
                    settings = new UserSettings { UserId = userId };
                    _context.UserSettings.Add(settings);
                }

                settings.AllowEmailNotifications = dto.AllowEmailNotifications;
                settings.CommentsOnYourPosts = dto.CommentsOnYourPosts;
                settings.CommentsThatMentionYou = dto.CommentsThatMentionYou;
                settings.PrivateCommentsOnWork = dto.PrivateCommentsOnWork;
                settings.WorkAndPostsFromTeachers = dto.WorkAndPostsFromTeachers;
                settings.ReturnedWorkAndGrades = dto.ReturnedWorkAndGrades;
                settings.InvitationsToJoinClasses = dto.InvitationsToJoinClasses;
                settings.DueDateReminders = dto.DueDateReminders;
                settings.LateSubmissions = dto.LateSubmissions;
                settings.Resubmissions = dto.Resubmissions;
                settings.InvitationsToCoteach = dto.InvitationsToCoteach;
                settings.ScheduledPostPublished = dto.ScheduledPostPublished;
                settings.ShowDisplayNameOnHomepage = dto.ShowDisplayNameOnHomepage;

                await _context.SaveChangesAsync();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
