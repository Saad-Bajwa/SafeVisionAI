namespace SafeVision_AI.API.Interfaces
{
    public interface IEmailNotificationService
    {
        Task<bool> SendEmail(string to, string subject, string body);
    }
}
