using System.Threading.Tasks;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
