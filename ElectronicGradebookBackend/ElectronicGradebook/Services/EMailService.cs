using ElectronicGradebook.Services.Interfaces;
using ElectronicGradebook.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ElectronicGradebook.Services
{
    public class EMailService : IEMailService
    {
        private readonly EMailSettings _eMailSettings;

        public EMailService(IOptions<EMailSettings> eMailSettings)
        {
            _eMailSettings = eMailSettings.Value;
        }

        public async Task sendEMailAsync(string subject, string to, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_eMailSettings.SenderDisplayName, _eMailSettings.SenderAddress));
            email.Sender = new MailboxAddress(_eMailSettings.SenderDisplayName, _eMailSettings.SenderAddress);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = body };

            var secureSocketOptions = SecureSocketOptions.None;
            if (_eMailSettings.EnableTLS) secureSocketOptions = SecureSocketOptions.StartTls;

            using var smtp = new SmtpClient();
            smtp.Connect(_eMailSettings.Host, _eMailSettings.Port, secureSocketOptions);
            if (_eMailSettings.EnableTLS) smtp.Authenticate(_eMailSettings.SenderAddress, _eMailSettings.SenderPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
