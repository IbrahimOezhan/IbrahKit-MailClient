using MailClient.Configs;
using MailClient.Toolkit.Utilities;

namespace MailClient.History
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

        public bool Validate(MessageConfig messageConfig)
        {
            bool result = history.Validate(messageConfig.GetRecipientAddresses());

            if (result)
            {
                return true;
            }

            if (MainUtilities.InputYesNo('Y', 'N', "Found duplicate adresses. Continue?", "Must provide valid input."))
            {
                return true;
            }

            return false;
        }
    }
}