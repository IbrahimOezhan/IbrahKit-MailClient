using MailClient.code.Exceptions;
using MailClient.code.Toolkit.CLI.Exceptions;
using MailClient.code.Toolkit.Utilities;
using System.Text.Json.Serialization;

namespace MailClient.code.Configs
{
    internal class MessageContentConfig
    {
        [JsonInclude]
        private string subject = string.Empty;

        [JsonInclude]
        private string body = string.Empty;

        public void ChooseBody(MessageContentBodyMode mode)
        {
            switch (mode)
            {
                case MessageContentBodyMode.PATH:

                    if (!File.Exists(body))
                    {
                        throw new InvalidConfigException();
                    }

                    string fileContent = File.ReadAllText(body);

                    if (StringUtilities.IsNullEmptyWhite(fileContent))
                    {
                        throw new InvalidConfigException();
                    }

                    body = fileContent;

                    break;
                case MessageContentBodyMode.URL:

                    var httpClient = new HttpClient();

                    body = httpClient.GetStringAsync(body).GetAwaiter().GetResult();

                    break;
            }
        }

        public string GetSubject() => subject;

        public string GetBody() => body;

        public bool Valid()
        {
            if (subject == string.Empty)
            {
                throw new CommandExecutionException("The subject in the provided message config is empty");
            }

            if (body == string.Empty)
            {
                throw new CommandExecutionException("The body in the provided message config is empty");
            }

            return true;
        }

        public enum MessageContentBodyMode
        {
            PATH,
            URL,
        }
    }
}
