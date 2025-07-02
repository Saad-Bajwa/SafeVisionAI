using System.Net.Mail;
using System.Net;
using SafeVision_AI.API.Interfaces;

namespace SafeVision_AI.API.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IConfiguration _configuration;
        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            try
            {
                var email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
                var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
                var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
                var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");
                var smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(email, password);
                var mailMessage = new MailMessage(email!, to, subject, body);
                mailMessage.IsBodyHtml = true;
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
