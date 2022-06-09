using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using MailKit;
//using System.Text.Json;

namespace AlumniTrackerSite.Data
{
    public static class Email
    {
        private static EmailData _data;
        private static SmtpClient _client;
        public static EmailData Initialize()
        {
            string path = (System.IO.Directory.GetCurrentDirectory() + @"\data\Settings.json");
            _client = new SmtpClient();
            return JsonConvert.DeserializeObject<EmailData>(File.ReadAllText(path));
        }
        public static async Task SendConfirmMessage(ILogger log, string returnURL, string reciever)
        {
            if (_data == null)
            { _data = Initialize(); }
            await SendMessage(log, reciever,_data.ConfirmSubject,_data.ConfirmBody + "\n\n" + returnURL);
        }
        public static async Task SendResetPasswordMessage(ILogger log, string returnURL, string reciever)
        {
            if (_data == null)
            { _data = Initialize(); }
            await SendMessage(log, reciever, _data.ResetSubject, _data.ResetBody + "\n\n" + returnURL);
        }
        //public static void AdminSendEmail(List<string> Recievers)
        //{

        //}
        public static async Task<string> SendMessage(ILogger log, string reciever, string subject, string body)
        {
            if(_data == null)
            { _data = Initialize(); }

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("AppTest", _data.AccountName));

            message.To.Add(MailboxAddress.Parse(reciever));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };
            string response;
            try
            {
                _client.Connect("smtp.gmail.com", 465, true);
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
            return response;

        }

        public static void Dispose()
        {
            _client.Disconnect(true);
            _client.Dispose();
        }

    }



    public class EmailData
    {
        public string ConfirmSubject { get; set; }
        public string ConfirmBody { get; set; }
        public string ResetSubject { get; set; }
        public string ResetBody { get; set; }
        public string Html { get; set; }
        public string AccountName { get; set; }
        public string AccountPass { get; set; }
    }
}


