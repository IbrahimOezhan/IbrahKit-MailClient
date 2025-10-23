using MailClient.Configs;
using MailClient.History;
using MailClient.Utilities;
using System.Net;
using System.Net.Mail;

namespace MailClient.Commands
{
    internal class SendCommand : Command
    {
        private SendContext context;

        public SendCommand(string[] args) : base(args)
        {
            this.context = new();
        }

        public SendCommand(string[] args, SendContext context) : base(args)
        {
            this.context = context;
        }

        private string Send()
        {
            ProfileConfig profileConfig = context.GetProfile();

            MessageConfig messageConfig = context.GetMessageConfig();

            ServerConfig serverConfig = context.GetServerConfig();

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

        public override string Run()
        {
            if (args.Length == 0)
            {
                return Send();
            }

            switch (args[0])
            {
                case "-server":
                case "-s":
                    if (args.Length > 1)
                    {
                        context.SetServer(args[1]);
                        return new SendCommand(args.Skip(2).ToArray(), context).Run();
                    }
                    else
                    {
                        return $"No value for {args[0]} parameter provided";
                    }
                case "-message":
                case "-m":
                    if (args.Length > 1)
                    {
                        context.SetMessage(args[1]);
                        return new SendCommand(args.Skip(2).ToArray(), context).Run();
                    }
                    else
                    {
                        return $"No value for {args[0]} parameter provided";
                    }
                default:

                    return $"{args[0]} is not a valid parameter";
            }
        }

        public override string CommandName()
        {
            return "send";
        }
    }
}
