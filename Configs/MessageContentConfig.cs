using MailClient.Exceptions;
using MailClient.Toolkit.Utilities;
using System.Text.Json.Serialization;

namespace MailClient.Configs
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
                MainUtilities.WriteLine("Subject is empty", ConsoleColor.Red);

                return false;
            }

            if (body == string.Empty)
            {
                MainUtilities.WriteLine("Body is empty", ConsoleColor.Red);

                return false;
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
