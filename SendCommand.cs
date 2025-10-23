using MailClient.Configs;
using MailClient.History;
using MailClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal class SendCommand : CommandState
    {
        private SendContext context;

        public SendCommand(string[] args, SendContext context) : base(args)
        {
            this.context = context;
        }

        private string Send()
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

        public override string Run()
        {
            if(args.Length == 0)
            {
                return Send();    
            }

            switch(args[0])
            {
                case "-server":
                case "-s":
                    if(args.Length > 1)
                    {
                        context.SetServer(args[1]);
                        return new SendCommand(args.Skip(2).ToArray(),context).Run();
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
                        return new SendCommand(args.Skip(2).ToArray(),context).Run();
                    }
                    else
                    {
                        return $"No value for {args[0]} parameter provided";
                    }
                default:

                    return $"{args[0]} is not a valid parameter";
            }  
        }
    }
}
