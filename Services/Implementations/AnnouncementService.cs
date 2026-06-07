using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.Announcement;
using GoogleClassroom.API.Models;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnnouncementResponseDto> CreateAnnouncementAsync(int classroomId, int userId, CreateAnnouncementDto dto)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null || !classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Classroom not found or you are not a teacher");

            var announcement = new Announcement
            {
                ClassroomId = classroomId,
                AuthorId = userId,
                Content = dto.Content,
                AttachmentUrl = dto.AttachmentUrl
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            // Load teacher for mapping
            await _context.Entry(announcement).Reference(a => a.Author).LoadAsync();

            return MapToResponse(announcement);
        }

        public async Task<IEnumerable<AnnouncementResponseDto>> GetClassroomAnnouncementsAsync(int classroomId, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You do not have access to this classroom");

            var announcements = await _context.Announcements
                .Include(a => a.Author)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .Where(a => a.ClassroomId == classroomId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return announcements.Select(MapToResponse);
        }

        public async Task<CommentResponseDto> AddCommentAsync(int announcementId, int userId, CreateCommentDto dto)
        {
            var announcement = await _context.Announcements
                .Include(a => a.Classroom)
                    .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == announcementId);

            if (announcement == null) throw new Exception("Announcement not found");

            if (announcement.Classroom.Members.All(m => m.UserId != userId))
                throw new Exception("You do not have access to comment on this announcement");

            var comment = new AnnouncementComment
            {
                AnnouncementId = announcementId,
                UserId = userId,
                Comment = dto.Comment
            };

            _context.AnnouncementComments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Entry(comment).Reference(c => c.User).LoadAsync();

            return new CommentResponseDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User.FirstName + " " + comment.User.LastName,
                Comment = comment.Comment,
                CreatedAt = DateTime.SpecifyKind(comment.CreatedAt, DateTimeKind.Utc)
            };
        }

        public async Task DeleteAnnouncementAsync(int announcementId, int userId)
        {
            var announcement = await _context.Announcements
                .Include(a => a.Classroom)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(a => a.Id == announcementId);

            if (announcement == null) throw new Exception("Announcement not found");

            if (announcement.AuthorId != userId && !announcement.Classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("You do not have permission to delete this announcement");

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
        }

        private AnnouncementResponseDto MapToResponse(Announcement announcement)
        {
            return new AnnouncementResponseDto
            {
                Id = announcement.Id,
                ClassroomId = announcement.ClassroomId,
                TeacherId = announcement.AuthorId, // DTO alias
                TeacherName = announcement.Author?.FirstName + " " + announcement.Author?.LastName,
                Content = announcement.Content,
                AttachmentUrl = announcement.AttachmentUrl,
                IsPinned = announcement.IsPinned,
                CreatedAt = DateTime.SpecifyKind(announcement.CreatedAt, DateTimeKind.Utc),
                Comments = announcement.Comments?.Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User?.FirstName + " " + c.User?.LastName,
                    Comment = c.Comment,
                    CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)
                }).ToList() ?? new List<CommentResponseDto>()
            };
        }
    }
}
