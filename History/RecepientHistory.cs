using IbrahKit_MailClient.Configs;

namespace IbrahKit_MailClient.History
{
    internal class RecepientHistory
    {
        private RecepientConfig recepient;
        private DateTime sentAt;

        public RecepientHistory()
        {

        }

        public RecepientHistory(RecepientConfig config)
        {
            recepient = config;
            sentAt = DateTime.UtcNow;
        }

        public RecepientConfig GetConfig() => recepient;

        public DateTime GetTime() => sentAt;
    }
}
