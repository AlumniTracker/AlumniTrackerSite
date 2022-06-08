using AlumniTrackerSite.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace AlumniTrackerSite.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private EmailData _emailData;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _emailData = Email.Initialize();
            AccountName = _emailData.AccountName;
            AccountPass = _emailData.AccountPass;
            _logger = logger;
        }

        //public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.
        public string AccountName { get; set; }
        public string AccountPass { get; set; }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(AccountName) || string.IsNullOrEmpty(AccountPass))
            {
                throw new Exception("Null UserAccount");
            }
            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            Email.SendMessage(toEmail, subject, message);

            //// Disable click tracking.
            //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //msg.SetClickTracking(false, false);
            //var response = await client.SendEmailAsync(msg);
            //_logger.LogInformation(response.IsSuccessStatusCode
            //                       ? $"Email to {toEmail} queued successfully!"
            //                       : $"Failure Email to {toEmail}");
        }
    }
}
