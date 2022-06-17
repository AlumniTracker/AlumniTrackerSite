using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using MailKit;
using MailKit.Security;
using MailKit.Net.Imap;
//using System.Text.Json;

namespace AlumniTrackerSite.Data
{
    public static class Email
    {
        private static EmailData _data;
        private static SmtpClient _client;
        /// <summary>
        /// Loads config file, and makes sure the instance data is around.
        /// </summary>
        /// <returns></returns>
        public static EmailData Initialize()
        {
            string path = (System.IO.Directory.GetCurrentDirectory() + @"\data\Settings.json");
            _client = new SmtpClient();
            return JsonConvert.DeserializeObject<EmailData>(File.ReadAllText(path));
        }
        /// <summary>
        /// a wrap around send message to attach the config Confirmation subject and body, and doesn't accept an input body
        /// </summary>
        /// <param name="log"></param>
        /// <param name="returnURL">String given by ASP.Net Identity code that contains an email confirmation url link</param>
        /// <param name="reciever"></param>
        /// <returns></returns>
        // Could later be configured to say if email sending was successful or not to user
        public static async Task SendConfirmMessage(ILogger log, string returnURL, string reciever)
        {
            if (_data == null)
            { _data = Initialize(); }
            await SendMessage(log, reciever,_data.ConfirmSubject,_data.ConfirmBody + "\n\n" + returnURL);
        }
        /// <summary>
        /// a wrap around send message to attach the config reset subject and body, and doesn't accept an input body
        /// </summary>
        /// <param name="log"></param>
        /// <param name="returnURL">String given by ASP.Net Identity code that contains reset link</param>
        /// <param name="reciever"></param>
        /// <returns></returns>
        // Could later be configured to say if email sending was successful or not to user
        public static async Task SendResetPasswordMessage(ILogger log, string returnURL, string reciever)
        {
            if (_data == null)
            { _data = Initialize(); }
            await SendMessage(log, reciever, _data.ResetSubject, _data.ResetBody + "\n\n" + returnURL);
        }
        //      Future plan to let admin send messages
        //public static void AdminSendEmail(List<string> Recievers)
        //{

        //}
        //      Future plan to let admin select many alumni to send messages to.
        //public static async Task<List<string>> SendManyMessages(ILogger log, List<string> recievers, string subject, string body)
        //{
        //    List<string> Responses = new List<string>();
        //    foreach (string reciever in recievers)
        //    {
                
        //    }
        //}
        /// <summary>
        /// Internal Message Sender meant only for email sent by code and not people
        /// </summary>
        /// <param name="log"></param>
        /// <param name="reciever"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task<string> SendMessage(ILogger log, string reciever, string subject, string body)
        {
            if(_data == null)
            { _data = Initialize(); }

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("CCFoundation", _data.AccountName));

            message.To.Add(MailboxAddress.Parse(reciever));

            message.Subject = subject;
            
            message.Body = new TextPart("html")
            {
                Text = body
            };
            string response;
            try
            {
                _client.Connect(_data.ServerName, _data.Port, true);
                _client.Authenticate(_data.AccountName, _data.AccountPass);
                response = await _client.SendAsync(message);
                
            }
            catch (Exception ex)
            {
                
                log.LogWarning("Caught Exception" + ex.Message);
                return "500 Internal Server Error";
            }
            bool result = response.Contains("2.0.0 OK");

            log.LogInformation(result
                                   ? $"Email to {reciever} queued successfully!"
                                   : $"Failure Email to {reciever}");
            //Dispose();
            return response;

        }
        /// <summary>
        /// Gets rid of loaded config data, and email connection.
        /// </summary>
        public static void Dispose()
        {
            
            _client.Disconnect(true);
            _client.Dispose();
            _data = null;
        }

    }


    /// <summary>
    /// Makes pulling data from JSON easier
    /// </summary>
    public class EmailData
    {
        public string ConfirmSubject { get; set; }
        public string ConfirmBody { get; set; }
        public string ResetSubject { get; set; }
        public string ResetBody { get; set; }
        public string Html { get; set; }
        public string AccountName { get; set; }
        public string AccountPass { get; set; }
        public int Port { get; set; }
        public string ServerName { get; set; }
    }
}


