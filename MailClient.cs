using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace MailClient
{
    internal class MailClient
    {
        public static string Run(string[] args)
        {
            MailClientMessageConfig? input = null;

            switch (args.Length)
            {
                case 0:
                    input = new MailClientMessageConfig();
                    input.Run().GetAwaiter().GetResult();
                    break;
                case 1:
                    input = JsonSerializer.Deserialize<MailClientMessageConfig>(args[0], MailClientUtilities.GetJsonOptions());
                    break;
                default:
                    throw new Exception("Cannot pass more than 1 arg");
            }

            if (!input.ValidateHistory(MailClientUtilities.GetHistory()))
            {
                Console.WriteLine("Found duplicate adresses. Continue?");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.KeyChar.ToString().ToLower() != "y")
                {
                    return "Operation Cancelled";
                }
            }

            StringBuilder historyContent = new(MailClientUtilities.GetHistory());

            MailClientServerConfig config;

            try
            {
                config = MailClientServerConfig.Get();
            }
            catch(Exception e)
            {
                return MailClientUtilities.FormattedException(e);
            }

            SmtpClient smtpClient = new(config.SMTP(), config.Port())
            {
                Credentials = new NetworkCredential(config.From(), config.Password()),
                EnableSsl = true
            };

            for (int i = 0; i < input.GetTos().Count; i++)
            {
                object[] format;

                format = input.GetTos()[i].GetFormattings().ToArray();

                MailMessage mail = new()
                {
                    From = new MailAddress(config.From()),

                    Subject = input.Subject(),

                    Body = string.Format(input.Body(), format),

                    IsBodyHtml = true
                };

                string adr = input.GetTos()[i].GetAdress();

                historyContent.AppendLine(adr.ToString() + " " + DateTime.Now);

                mail.To.Add(adr);

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(MailClientUtilities.FormattedException(ex));
                }
            }

            using (StreamWriter sw = new(MailClientUtilities.GetHistoryPath()))
            {
                sw.Write(historyContent);
            }

            return "Success";
        }
    }
}
