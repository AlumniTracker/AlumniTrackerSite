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

        public async Task SendConfirmAsync(string toEmail, string returnUrl)
        {
            await Email.SendConfirmMessage(_logger, returnUrl, toEmail);
        }
        public async Task SendResetAsync(string toEmail, string returnUrl)
        {
            await Email.SendResetPasswordMessage(_logger, returnUrl, toEmail);
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(AccountName) || string.IsNullOrEmpty(AccountPass))
            {
                throw new Exception("Null UserAccount");
            }
            switch (subject)
            {
                case "Email.Confirmation":
                    await SendConfirmAsync(toEmail, message);
                        break;
                case "Email.Reset":
                    await SendResetAsync(toEmail, message);
                        break;
            }
            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            string response = await Email.SendMessage(_logger, toEmail, subject, message);

            //EventId result = new EventId(200);
            //// Disable click tracking.
            //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //msg.SetClickTracking(false, false);
            //var response = await client.SendEmailAsync(msg);
            //
        }
    }
}
