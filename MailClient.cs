using System.Net;
using System.Net.Mail;

namespace MailClient
{
    internal class MailClient
    {
        public static string Run(string[] args)
        {
            try
            {
                HistoryHandler history = new();

                MessageConfig msgConfig = MessageConfig.Get(args);

                if (!history.Validate(msgConfig.GetRecipients().Select(x => x.GetAdress()).ToList()))
                {
                    return "Operation Cancelled";
                }

                ServerConfig serverConfig = ServerConfig.Get();

                SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
                {
                    Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),

                    EnableSsl = true
                };

                for (int i = 0; i < msgConfig.GetRecipients().Count; i++)
                {
                    object[] placeholderFormattings = msgConfig.GetRecipients()[i].GetFormattings().ToArray();

                    string toAdress = msgConfig.GetRecipients()[i].GetAdress();

                    MailMessage mail = new(serverConfig.From(), toAdress, msgConfig.Content().Subject(), string.Format(msgConfig.Content().Body(), placeholderFormattings))
                    {
                        IsBodyHtml = true
                    };

                    smtpClient.Send(mail);

                    history.AddToHistory(toAdress);

                    Utilities.WriteLine($"Sent mail to {toAdress} successfully", ConsoleColor.Green);
                }

                history.SaveHistory();

                return "Success";
            }
            catch
            {
                throw;
            }
        }
    }
}
