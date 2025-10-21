using MailClient.Configs;
using MailClient.History;
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
                switch(args.Length)
                {
                    case 0:
                        break;
                    case 3:
                        args[0] = Utilities.ForceInput("Select Profile", "You must select a profile");
                        args[1] = Utilities.ForceInput("Enter Server Config Path", "You must enter a path");
                        args[2] = Utilities.ForceInput("Select Message Config Path", "You must enter a path");
                        break;
                    default:
                        throw new InvalidDataException("Invalid amount of arguments");
                }

                MessageConfig msgConfig = MessageConfig.Get(args[0]);

                ServerConfig serverConfig = ServerConfig.Get(args[1]);

                HistoryHandler historyHandler = new();

                if (!historyHandler.Validate(msgConfig.GetRecipients().Select(x => x.GetAdress()).ToList()))
                {
                    return "Operation Cancelled";
                }

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

                    historyHandler.AddToHistory(toAdress);

                    Utilities.WriteLine($"Sent mail to {toAdress} successfully", ConsoleColor.Green);
                }

                historyHandler.SaveHistory();

                return "Success";
            }
            catch
            {
                throw;
            }
        }
    }
}
