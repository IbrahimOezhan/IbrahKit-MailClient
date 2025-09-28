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
            MailClientMessageConfig? msgConfig = null;

            switch (args.Length)
            {
                case 0:
                    msgConfig = new MailClientMessageConfig();
                    msgConfig.Run().GetAwaiter().GetResult();
                    break;
                case 1:
                    msgConfig = JsonSerializer.Deserialize<MailClientMessageConfig>(args[0], MailClientUtilities.GetJsonOptions());
                    break;
                default:
                    return "Error: Cannot pass more than 1 args when launching the tool";
            }

            if(msgConfig == null)
            {
                return "Error: Message config is null";
            }

            if (!msgConfig.ValidateHistory(MailClientUtilities.GetHistory()))
            {
                Console.WriteLine("Found duplicate adresses. Continue?");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.KeyChar.ToString().ToLower() != "y")
                {
                    return "Operation Cancelled";
                }
            }

            StringBuilder historyContent = new(MailClientUtilities.GetHistory());

            MailClientServerConfig serverConfig;

            try
            {
                serverConfig = MailClientServerConfig.Get();
            }
            catch(Exception e)
            {
                return MailClientUtilities.FormattedException(e);
            }

            SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
            {
                Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),
                EnableSsl = true
            };

            for (int i = 0; i < msgConfig.GetTos().Count; i++)
            {
                object[] format;

                format = msgConfig.GetTos()[i].GetFormattings().ToArray();

                MailMessage mail = new()
                {
                    From = new MailAddress(serverConfig.From()),

                    Subject = msgConfig.Subject(),

                    Body = string.Format(msgConfig.Body(), format),

                    IsBodyHtml = true
                };

                string adr = msgConfig.GetTos()[i].GetAdress();

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
