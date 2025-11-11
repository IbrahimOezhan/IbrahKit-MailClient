using IbrahKit_MailClient.Configs;

namespace IbrahKit_MailClient.History
{
    internal class RecepientHistory
    {
        private RecipientConfig recepient;
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
