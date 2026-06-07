using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.Notification;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications.Select(n => new NotificationResponseDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                throw new Exception("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
