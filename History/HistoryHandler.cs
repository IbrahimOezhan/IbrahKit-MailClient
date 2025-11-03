using IbrahKit_MailClient.Configs;
using IbrahKit_MailClient.Utilities;

namespace IbrahKit_MailClient.History
{
    internal class HistoryHandler
    {
        private History history = new();

        public HistoryHandler(ProfileConfig profile)
        {
            history = profile.GetHistory();
        }

        public void AddToHistory(string adress)
        {
            history.AddToHistory(adress);
        }

        public bool Validate(MessageConfig messageConfig,bool inc, bool skip)
        {
            bool result = history.Validate(messageConfig.GetRecipients(), inc, skip);

            if (result)
            {
                return true;
            }

            throw new IbrahKit_CLI.Exceptions.CommandExecutionException("Found duplicate addresses. Use --includeDuplicates or --skipDuplicates");
        }
    }
}