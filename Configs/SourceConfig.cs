
using IbrahKit_CLI.Exceptions;
using IbrahKit_MailClient.Utilities;
using System.Text.Json.Serialization;
using Utilities;

namespace IbrahKit_MailClient.Configs
{
    internal class SourceConfig
    {
        [JsonInclude]
        private string subject = string.Empty;

        [JsonInclude]
        private string body = string.Empty;

        public static SourceConfig Get(string path, SourceConfig.MessageContentBodyMode bodyMode)
        {
            bool suc = JsonUtilities.TryDeserialize<SourceConfig>(path, out SourceConfig result);

            if (!suc)
            {
                throw new CommandExecutionException($"Couldnt find or deserialize the source config at {path}")
            }

            result.Validate(bodyMode).GetAwaiter().GetResult();

            return result;
        }

        public async Task Validate(MessageContentBodyMode mode)
        {
            switch (mode)
            {
                case MessageContentBodyMode.PATH:

                    if (!File.Exists(body))
                    {
                        throw new CommandExecutionException();
                    }

                    string fileContent = File.ReadAllText(body);

                    if (StringUtilities.IsNullEmptyWhite(fileContent))
                    {
                        throw new CommandExecutionException();
                    }

                    body = fileContent;

                    break;
                case MessageContentBodyMode.URL:

                    var httpClient = new HttpClient();

                    body = await httpClient.GetStringAsync(body);

                    break;
            }

            if (subject == string.Empty)
            {
                throw new CommandExecutionException("The subject in the provided message config is empty");
            }

            if (body == string.Empty)
            {
                throw new CommandExecutionException("The body in the provided message config is empty");
            }
        }

        public string GetSubject() => subject;

        public string GetBody() => body;

        public enum MessageContentBodyMode
        {
            PATH,
            URL,
        }
    }
}
