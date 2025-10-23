using MailClient.Configs;

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

            Console.Write("Found duplicate adresses. Continue? Y/y (Yes) Anything Else (No): ");

            ConsoleKeyInfo key = Console.ReadKey();

            Console.WriteLine();

            if (!key.KeyChar.ToString().Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}