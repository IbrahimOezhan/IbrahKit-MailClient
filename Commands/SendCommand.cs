using MailClient.Configs;
using MailClient.History;
using MailClient.Toolkit.CLI;
using MailClient.Toolkit.CLI.Exceptions;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailClient.Commands
{
    internal class SendCommand : Command<SendContext, SendCommand>
    {
        public SendCommand(string[] args) : base(args) { }

        public SendCommand(string[] args, SendContext context) : base(args, context) { }

        public override string Execute()
        {
            StringBuilder sb = new();

            ProfileConfig profileConfig = GetContext().GetProfile();

            MessageConfig messageConfig = GetContext().GetMessageConfig();

            Console.Write(1);

            ServerConfig serverConfig = GetContext().GetServerConfig();

            Console.Write(2);

            HistoryHandler historyHandler = new(profileConfig);

            if (!historyHandler.Validate(messageConfig)) return "Operation Cancelled";

            SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
            {
                Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),

                EnableSsl = true
            };

            for (int i = 0; i < messageConfig.GetRecipients().Count; i++)
            {
                object[] placeholderFormattings = [.. messageConfig.GetRecipients()[i].GetFormattings()];

                string toAdress = messageConfig.GetRecipients()[i].GetAdress();

                MailMessage mail = new(serverConfig.From(), toAdress, messageConfig.Content().GetSubject(), string.Format(messageConfig.Content().GetBody(), placeholderFormattings))
                { IsBodyHtml = true };

                smtpClient.Send(mail);

                historyHandler.AddToHistory(toAdress);

                sb.AppendLine($"Sent mail to {toAdress} successfully");
            }

            profileConfig.SaveConfig();

            return sb.ToString();
        }

        public override (string,string,List<Argument>) GetData()
        {
            return
            ("send", "sends e-mails to specified recepients",
            [
                new((args) =>
                {
                    GetContext().SetMessage(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the path to the message config file","-m","-message"),
                new((args) =>
                {
                    if (!Enum.TryParse(args[1],true, out MessageContentConfig.MessageContentBodyMode result))
                    {
                        throw new ArgumentParsingException();
                    }

                    GetContext().SetBodyMode(result);

                    return ARG_PROCESS_SUCCES;

                },"Set the mode of the body source","-b","-body"),
                new((args)=>
                {
                    GetContext().SetServer(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the path to the server config file","-s","-server"),
                new((args) =>
                {
                    GetContext().SetProfile(args[1]);
                    return ARG_PROCESS_SUCCES;

                },"Set the profile to use","-p","-profile")
            ]);
        }
    }
}