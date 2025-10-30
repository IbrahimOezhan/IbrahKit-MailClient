using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.Configs
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
                recipients[i].Valid(contentConfig);
            }

            return result;
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => recipients;
    }
}