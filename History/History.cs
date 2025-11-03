using IbrahKit_MailClient.Configs;
using IbrahKit_MailClient.Utilities;
using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.History
{
    internal class History
    {
        [JsonInclude]
        private List<HistoryElement> historyElements = new();

        public void AddToHistory(string adress)
        {
            historyElements.Add(new(adress, DateTime.Now));
        }

        public bool Validate(List<MessageRecepientConfig> addresses, bool inc, bool skip)
        {
            bool result = true;

            for (int i = addresses.Count - 1; i >= 0; i--)
            {
                if(historyElements.Select(x => x.Adress()).Contains(addresses[i].GetAddress()))
                {
                    if (inc) continue;
                    if(skip)
                    {
                        addresses.RemoveAt(i);
                        continue;
                    }

                    MainUtilities.WriteLine($"Warning: {addresses[i].GetAddress()} was already used to send a mail", ConsoleColor.Yellow);
                    result = false;
                }
            }


            return result;
        }
    }
}
