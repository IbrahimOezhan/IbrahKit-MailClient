using MailClient.Exceptions;
using MailClient.Toolkit.Utilities;
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
            if (!contentConfig.Valid()) return false;

            if (!recipientsConfig.Valid(contentConfig)) return false;

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
                throw new InvalidConfigException();
            }

            string fileContent = File.ReadAllText(path);

            if (StringUtilities.IsNullEmptyWhite(fileContent) || fileContent == null)
            {
                throw new InvalidConfigException();
            }

            try
            {
                config = JsonSerializer.Deserialize<MessageConfig>(fileContent, MainUtilities.GetJsonOptions());
            }
            catch
            {
                throw new InvalidConfigException();
            }

            if (config == null)
            {
                throw new InvalidConfigException();
            }

            if (!config.Valid())
            {

            }

            config.Content().ChooseBody(bodyMode);

            return config;
        }
    }
}