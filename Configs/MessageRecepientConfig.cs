using System.Text.Json.Serialization;

namespace MailClient.Configs
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
            address = adress;
            this.formattings = formattings;
        }

        public bool Valid()
        {
            bool result = address != string.Empty;

            if (!result) Utilities.WriteLine("Adress is empty", ConsoleColor.Red);

            return result;
        }

        public string GetAdress() => address;

        public List<string> GetFormattings() => formattings;
    }
}