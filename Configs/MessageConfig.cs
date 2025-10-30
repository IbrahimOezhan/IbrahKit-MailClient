
using IbrahKit_CLI.Exceptions;
using MailClient.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailClient.Configs
{
    internal class MessageConfig
    {
        [JsonInclude]
        private MessageContentConfig contentConfig = new();

        [JsonInclude]
        private MessageRecepientsConfig recipientsConfig = new();

        public bool Valid()
        {
            contentConfig.Valid();

            recipientsConfig.Valid(contentConfig);

            return true;
        }

        public MessageContentConfig Content() => contentConfig;

        public List<MessageRecepientConfig> GetRecipients() => recipientsConfig.GetRecepientConfigs();

        public List<string> GetRecipientAddresses() => [.. recipientsConfig.GetRecepientConfigs().Select(x => x.GetAdress())];

        public static MessageConfig Get(string path, MessageContentConfig.MessageContentBodyMode bodyMode)
        {
            MessageConfig? config;

            if (!File.Exists(path))
            {
                throw new CommandExecutionException($"There is no message config file at {path}");
            }

            string fileContent = File.ReadAllText(path);

            if (StringUtilities.IsNullEmptyWhite(fileContent) || fileContent == null)
            {
                throw new CommandExecutionException($"The messgae config file's contents are empty or null at {path}");
            }

            try
            {
                config = JsonSerializer.Deserialize<MessageConfig>(fileContent, MainUtilities.GetJsonOptions());
            }
            catch
            {
                throw new CommandExecutionException($"An exception happened during the deserialization of the message config at {path}");
            }

            if (config == null)
            {
                throw new CommandExecutionException($"The deserialization of the message config at {path} resulted in a null value");
            }

            config.Valid();

            config.Content().ChooseBody(bodyMode);

            return config;
        }
    }
}