using IbrahKit_MailClient.Configs;
using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.History
{
    internal class History
    {
        [JsonInclude]
        private List<RecepientHistory> historyElements = new();

        public void AddToHistory(RecipientConfig recepient)
        {
            historyElements.Add(new(recepient));
        }

        public bool Validate(List<RecipientConfig> addresses, bool inc, bool skip, out List<RecepientHistory> alreadyUsed)
        {
            bool result = true;

            List<RecepientHistory> _alreadyUsed = new();

            alreadyUsed = new();

            if (inc)
            {
                return result;
            }

            addresses.ForEach((y) =>
            {
                IEnumerable<RecepientHistory> his = historyElements.Where(x => x.GetConfig().GetAddress().Equals(y.GetAddress(), StringComparison.OrdinalIgnoreCase));

                _alreadyUsed.AddRange(his);
            });

            alreadyUsed = _alreadyUsed;

            if (skip)
            {
                addresses.RemoveAll(address => _alreadyUsed.Select(already => already.GetConfig()).Contains(address));

                return true;
            }

            return alreadyUsed.Count == 0;
        }
    }
}