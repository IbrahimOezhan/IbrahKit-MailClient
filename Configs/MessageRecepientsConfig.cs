using System.Text.Json.Serialization;

namespace MailClient.Configs
{
    internal class MessageRecepientsConfig
    {
        [JsonInclude]
        private List<MessageRecepientConfig> recipients = new();

        public bool Valid(MessageContentConfig contentConfig)
        {
            bool result = true;

            for (int i = 0; i < recipients.Count; i++)
            {
                if (!recipients[i].Valid(contentConfig)) result = false;
            }

            return result;
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => recipients;
    }
}