using MailClient.code.Toolkit.Utilities;
using System.Text.Json.Serialization;

namespace MailClient.code.History
{
    internal class History
    {
        [JsonInclude]
        private List<HistoryElement> historyElements = new();

        public void AddToHistory(string adress)
        {
            historyElements.Add(new(adress, DateTime.Now));
        }

        public bool Validate(List<string> adresses)
        {
            bool result = true;

            for (int i = 0; i < adresses.Count; i++)
            {
                MainUtilities.WriteLine($"Warning: {adresses[i]} was already used to send a mail", ConsoleColor.Yellow);

                if (historyElements.Select(x => x.Adress()).Contains(adresses[i]))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
