using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.Deprecated
{
    internal class HistoryElement(string adress, DateTime timeSent)
    {
        [JsonInclude]
        private string adress = adress;

        [JsonInclude]
        private DateTime timeSent = timeSent;

        public string Adress() => adress;

        public DateTime Time() => timeSent;
    }
}
