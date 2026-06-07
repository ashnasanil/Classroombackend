using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Notification;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
    }
}
