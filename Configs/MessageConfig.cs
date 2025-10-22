using MailClient.Exceptions;
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
            if (!contentConfig.Valid()) return false;

            if (!recipientsConfig.Valid(contentConfig)) return false;

            return true;
        }

        public MessageContentConfig Content() => contentConfig;

        public List<MessageRecepientConfig> GetRecipients() => recipientsConfig.GetRecepientConfigs();
        public List<string> GetRecipientAddresses() => recipientsConfig.GetRecepientConfigs().Select(x => x.GetAdress()).ToList();

        public static MessageConfig Get(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("Path doesnt exist");
            }

            string json = File.ReadAllText(path);

            if(StringUtilities.IsNullEmptyWhite(json))
            {
                throw new FileEmptyException();
            }

            MessageConfig? msgConfig = JsonSerializer.Deserialize<MessageConfig>(json, MainUtilities.GetJsonOptions());

            if (msgConfig == null) throw new NullReferenceException("Message Comfig is null");

            msgConfig.Content().ConvertURLToHTML();

            if (!msgConfig.Valid())
            {
                throw new ConfigInvalidException();
            }

            return msgConfig;
        }
    }
}