using MimeKit;
using MailKit.Net.Smtp;

namespace AlumniTrackerSite.Data
{
    public class Email
    {

        public static void SendMessage() {

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("AppTest", "hate4g@gmail.com"));

            message.To.Add(MailboxAddress.Parse("hate4g@gmail.com"));

            message.Subject = "What are you gunna do? Cry??";

            message.Body = new TextPart("plain")
            {
                Text = @"Yup. Cry about it, Beaoch"
            };

            Console.Write("Email: ");
            string emailAddress = Console.ReadLine();

            //pgkjlkmeqblofudf
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailAddress, pass);
                client.Send(message);

                Console.WriteLine("Email Sent!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

        }

    }
}
