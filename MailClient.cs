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

                ServerConfig serverConfig = ServerConfig.Get();

                if (!history.Validate(msgConfig.GetTos().Select(x => x.GetAdress()).ToList())) return "Operation Cancelled";

                SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
                {
                    Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),

                    EnableSsl = true
                };

                for (int i = 0; i < msgConfig.GetTos().Count; i++)
                {
                    object[] placeholderFormattings = msgConfig.GetTos()[i].GetFormattings().ToArray();

                    MailMessage mail = new()
                    {
                        From = new MailAddress(serverConfig.From()),

                        Subject = msgConfig.Content().Subject(),

                        Body = string.Format(msgConfig.Content().Body(), placeholderFormattings),

                        IsBodyHtml = true
                    };

                    string toAdress = msgConfig.GetTos()[i].GetAdress();

                    history.AddToHistory(toAdress);

                    mail.To.Add(toAdress);

                    smtpClient.Send(mail);
                }

                history.SaveHistory();

                return "Success";
            }
            catch (Exception e)
            {
                return Utilities.FormattedException(e);
            }
        }
    }
}
