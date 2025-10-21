using MailClient.Exceptions;
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
            if(!contentConfig.Valid()) return false;

            if(!recipientsConfig.Valid(contentConfig)) return false;

            return true;
        }

        public MessageContentConfig Content() => contentConfig;

        public List<MessageRecepientConfig> GetRecipients() => recipientsConfig.GetRecepientConfigs();

        public static MessageConfig Get(string path)
        {
            MessageConfig? msgConfig;

            string json;

            if (!File.Exists(path))
            {
                throw new ArgumentException("Path doesnt exist");
            }

            json = File.ReadAllText(path);

            msgConfig = JsonSerializer.Deserialize<MessageConfig>(json, Utilities.GetJsonOptions());

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