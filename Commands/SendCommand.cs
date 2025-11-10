using IbrahKit_CLI;
using IbrahKit_CLI.Exceptions;
using IbrahKit_CLI.Params;

using IbrahKit_MailClient.Configs;
using IbrahKit_MailClient.History;

using System.Net;
using System.Net.Mail;
using System.Text;

namespace IbrahKit_MailClient.Commands
{
    internal class SendCommand : Command<SendContext, SendCommand>
    {
        public SendCommand(string[] args) : base(args) { }

        public SendCommand(string[] args, SendContext context) : base(args, context) { }

        public override string Execute()
        {
            StringBuilder sb = new();

            ProfileConfig profileConfig = GetContext().GetProfile();

            SourceConfig sourceConfig = GetContext().GetSourceConfig();

            RecepientsConfig recepientsConfig = GetContext().GetRecConfig(sourceConfig);

            ServerConfig serverConfig = GetContext().GetServerConfig();

            HistoryHandler historyHandler = new(profileConfig);

            if (!historyHandler.Validate(recepientsConfig, GetContext().GetInclude(), GetContext().GetSkip(), out List<RecepientHistory> alreadyUsed))
            {
                StringBuilder errMsg = new("Found email addresses that were already in use: ");

                foreach (var item in alreadyUsed)
                {
                    errMsg.AppendLine(alreadyUsed.ToString());
                }

                errMsg.AppendLine("Use --includeDuplicates or --skipDuplicates");

                throw new CommandExecutionException(errMsg.ToString());
            }

            SmtpClient smtpClient = new(serverConfig.SMTP(), serverConfig.Port())
            {
                Credentials = new NetworkCredential(serverConfig.From(), serverConfig.Password()),

                EnableSsl = true
            };

            for (int i = 0; i < recepientsConfig.GetRecepientConfigs().Count; i++)
            {
                object[] placeholderFormattings = [.. recepientsConfig.GetRecepientConfigs()[i].GetFormattings()];

                string toAdress = recepientsConfig.GetRecepientConfigs()[i].GetAddress();

                string body = string.Format(sourceConfig.GetBody(), placeholderFormattings);

                MailMessage mail = new(
                    serverConfig.From(), toAdress, sourceConfig.GetSubject(), body)
                { IsBodyHtml = true };

                smtpClient.Send(mail);

                historyHandler.AddToHistory(toAdress);

                sb.AppendLine($"Sent mail to {toAdress} successfully");
            }

            profileConfig.SaveConfig();

            return sb.ToString();
        }

        public override (string, string, List<Param>) GetData()
        {
            return
            ("send", "sends e-mails to specified recepients",
            [
                new Argument((args) =>
                {
                    GetContext().SetMessage(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the path to the message config file","-m","-message"),

                new Argument((args) =>
                {
                    GetContext().SetRecepients(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the path to the recepient config file","-r","-recepients"),

                new Argument((args) =>
                {
                    if (!Enum.TryParse(args[1],true, out SourceConfig.MessageContentBodyMode result))
                    {
                        throw new ArgumentParsingException();
                    }

                    GetContext().SetBodyMode(result);

                    return ARG_PROCESS_SUCCES;

                },"Set the mode of the body source","-b","-body"),
                new Argument((args)=>
                {
                    GetContext().SetServer(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the path to the server config file","-s","-server"),
                new Argument((args) =>
                {
                    GetContext().SetProfile(args[1]);
                    return ARG_PROCESS_SUCCES;

                },"Set the profile to use","-p","-profile"),
                new Flag((args)=>
                {
                    GetContext().Include();
                    return ARG_PROCESS_SUCCES;

                },"Include Duplicates","--includeDuplicates"),
                new Flag((args)=>
                {
                    GetContext().Skip();
                    return ARG_PROCESS_SUCCES;

                },"Skip Duplicates","--skipDuplicates")
            ]);
        }
    }
}