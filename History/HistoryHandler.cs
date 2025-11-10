using IbrahKit_MailClient.Configs;

namespace IbrahKit_MailClient.History
{
    internal class HistoryHandler
    {
        private History history = new();

        public HistoryHandler(ProfileConfig profile)
        {
            history = profile.GetHistory();
        }

        public void AddToHistory(RecepientConfig config)
        {
            history.AddToHistory(config);
        }

        public bool Validate(RecepientsConfig recepientsConfig, bool inc, bool skip, out List<RecepientHistory> alreadyUsed)
        {
            return history.Validate(recepientsConfig.GetRecepientConfigs(), inc, skip, out alreadyUsed);
        }
    }
}