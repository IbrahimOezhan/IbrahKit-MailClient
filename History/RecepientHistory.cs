using IbrahKit_MailClient.Configs;
using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.History
{
    internal class RecepientHistory
    {
        [JsonInclude]
        private RecipientConfig recepient;

        [JsonInclude]
        private DateTime sentAt;

        public RecepientHistory()
        {

        }

        public RecepientHistory(RecipientConfig config)
        {
            recepient = config;
            sentAt = DateTime.UtcNow;
        }

        public RecipientConfig GetConfig() => recepient;

        public DateTime GetTime() => sentAt;
    }
}
