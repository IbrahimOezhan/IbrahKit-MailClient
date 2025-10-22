using MailClient.Configs;
using MailClient.History;
using MailClient.Utilities;
using System.Net;
using System.Net.Mail;

namespace MailClient
{
    internal class MailClient
    {
        public const string FOLDER = "IbrahKit";

        private Dictionary<string, Action<string[]>> commands = new();

        public string Run(string[] args)
        {
            commands.Add("send", (arguments) =>
            {
                Send(arguments);
            });

            commands.Add("profile", (arguments) =>
            {
                Profile(arguments);
            });

            try
            {
                switch(args.Length)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                }


                return Send(args);
            }
            catch
            {
                throw;
            }
        }

        private static string Profile(string[] args)
        {
            if(args.Length == 0)
            {
                return "profile command requires arguments. Possible are:";
            }
            else
            {

            }

                return "";
        }

        private static string Send(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    args = new string[3];
                    args[0] = MainUtilities.ForceInput("Select Profile: ", "You must select a profile");
                    args[1] = MainUtilities.ForceInput("Select Message Config Path: ", "You must enter a path");
                    args[2] = MainUtilities.ForceInput("Enter Server Config Path: ", "You must enter a path");
                    break;
                case 3:
                    break;
                default:
                    throw new InvalidDataException("Invalid amount of arguments");
            }

            ProfileConfig profileConfig = ProfileConfig.Create(args[0]);

            MessageConfig messageConfig = MessageConfig.Get(args[1]);

            ServerConfig serverConfig = ServerConfig.Get(args[2]);

            HistoryHandler historyHandler = new(profileConfig);

            if (!historyHandler.Validate(messageConfig)) return "Operation Cancelled";

            SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
            {
                Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),

                EnableSsl = true
            };

            for (int i = 0; i < messageConfig.GetRecipients().Count; i++)
            {
                object[] placeholderFormattings = messageConfig.GetRecipients()[i].GetFormattings().ToArray();

                string toAdress = messageConfig.GetRecipients()[i].GetAdress();

                MailMessage mail = new(serverConfig.From(), toAdress, messageConfig.Content().Subject(), string.Format(messageConfig.Content().Body(), placeholderFormattings))
                {
                    IsBodyHtml = true
                };

                smtpClient.Send(mail);

                historyHandler.AddToHistory(toAdress);

                MainUtilities.WriteLine($"Sent mail to {toAdress} successfully", ConsoleColor.Green);
            }

            profileConfig.Save();

            return "Success";
        }
    }
}
