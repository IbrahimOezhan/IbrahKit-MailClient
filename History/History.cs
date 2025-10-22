using MailClient.Utilities;
using System.Text.Json.Serialization;

namespace MailClient.History
{
    internal class History
    {
        [JsonInclude]
        private List<HistoryElement> history = new();

        public void AddToHistory(string adress)
        {
            history.Add(new(adress, DateTime.Now));
        }

        public bool Validate(List<string> adresses)
        {
            bool result = true;

            for (int i = 0; i < adresses.Count; i++)
            {
                MainUtilities.WriteLine($"Warning: {adresses[i]} was already used to send a mail", ConsoleColor.Yellow);

                if (history.Select(x => x.Adress()).Contains(adresses[i]))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
