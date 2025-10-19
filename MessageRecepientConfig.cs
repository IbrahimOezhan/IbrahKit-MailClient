using System.Text.Json.Serialization;

namespace MailClient
{
    public class MessageRecepientConfig
    {
        [JsonInclude]
        private string address = string.Empty;

        [JsonInclude]
        private List<string> formattings = new();


        public MessageRecepientConfig()
        {

        }

        public MessageRecepientConfig(string adress, List<string> formattings)
        {
            this.address = adress;
            this.formattings = formattings;
        }

        public string GetAdress() => address;

        public List<string> GetFormattings() => formattings;
    }
}