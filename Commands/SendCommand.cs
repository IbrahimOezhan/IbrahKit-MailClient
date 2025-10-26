using MailClient.Configs;
using MailClient.Exceptions;
using MailClient.History;
using MailClient.Toolkit.CLI;
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

            ServerConfig serverConfig = GetContext().GetServerConfig();

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

        public override string GetCommand()
        {
            return "send";
        }

        public override List<Argument> GetArguments()
        {
            return
            [
                new((args) =>
                {
                    if (args.Length == 1)
                    {
                        return $"No value for {args[0]} parameter provided";
                    }

                    GetContext().SetMessage(args[1]);

                    return new SendCommand([.. args.Skip(2)], GetContext()).Parse();
                },"-m","-message"),
                new((args) =>
                {
                    if (args.Length == 1)
                    {
                        return $"No value for {args[0]} parameter provided";
                    }

                    if (!Enum.TryParse(args[1],true, out MessageContentConfig.MessageContentBodyMode result))
                    {
                        throw new InvalidConfigException();
                    }

                    GetContext().SetBodyMode(result);

                    return string.Empty;
                },"-b","-body"),
                new((args)=>
                {
                    if (args.Length == 1)
                    {
                        return $"No value for {args[0]} parameter provided";
                    }

                    GetContext().SetServer(args[1]);

                    return string.Empty;
                },"-s","-server"),
            ];
        }
    }
}