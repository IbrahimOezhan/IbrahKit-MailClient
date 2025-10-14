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
            try
            {
                MailClientMessageConfig msgConfig = GetMsgConfig(args);
                
                MailClientServerConfig serverConfig = MailClientServerConfig.Get();

                if(!ValidateHistory(msgConfig)) return "Operation Cancelled";

                StringBuilder historyContent = new(MailClientUtilities.GetHistory());

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

                        Subject = msgConfig.Subject(),

                        Body = string.Format(msgConfig.Body(), placeholderFormattings),

                        IsBodyHtml = true
                    };

                    string toAdress = msgConfig.GetTos()[i].GetAdress();

                    historyContent.AppendLine(toAdress.ToString() + ";" + DateTime.Now);

                    mail.To.Add(toAdress);

                    smtpClient.Send(mail);
                }

                MailClientUtilities.SaveHistory(historyContent);

                return "Success";
            }
            catch (Exception e)
            {
                return MailClientUtilities.FormattedException(e);
            }
        }

        private static bool ValidateHistory(MailClientMessageConfig msgConfig)
        {
            if (!msgConfig.ValidateHistory(MailClientUtilities.GetHistory()))
            {
                Console.WriteLine("Found duplicate adresses. Continue?");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.KeyChar.ToString().ToLower() != "y")
                {
                    return false;
                }
            }
            return true;
        }

        private static MailClientMessageConfig GetMsgConfig(string[] args)
        {
            MailClientMessageConfig? msgConfig;

            if (args.Length > 0)
            {
                string json;

                switch (args[0])
                {
                    case "path":

                        if (args.Length != 2)
                        {
                            throw new ArgumentException("Invalid arguments for path command");
                        }

                        if (!File.Exists(args[1]))
                        {
                            throw new ArgumentException("Path doesnt exist");
                        }

                        json = File.ReadAllText(args[0]);

                        break;
                    case "text":

                        if (args.Length != 2)
                        {
                            throw new ArgumentException("Invalid arguments for text command");
                        }

                        json = args[1];

                        break;
                    default:

                        throw new ArgumentException(args[0] + " is an invald argument");
                }

                msgConfig = JsonSerializer.Deserialize<MailClientMessageConfig>(json, MailClientUtilities.GetJsonOptions());
            }
            else
            {
                msgConfig = new MailClientMessageConfig();
                msgConfig.Run().GetAwaiter().GetResult();
            }

            if (msgConfig == null)
            {
                throw new NullReferenceException("Message config is null");
            }

            return msgConfig;
        }
    }
}
