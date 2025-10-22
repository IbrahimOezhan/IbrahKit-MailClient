using MailClient.Utilities;
using System.Text.Json.Serialization;

namespace MailClient.Configs
{
    public class MessageRecepientConfig
    {
        [JsonInclude]
        private string toAddress = string.Empty;

        [JsonInclude]
        private List<string> formattings = new();


        public MessageRecepientConfig()
        {

        }

        public MessageRecepientConfig(string adress, List<string> formattings)
        {
            toAddress = adress;
            this.formattings = formattings;
        }

        public bool Valid()
        {
            bool result = toAddress != string.Empty;

            if (!result) MainUtilities.WriteLine("Adress is empty", ConsoleColor.Red);

            return result;
        }

        public string GetAdress() => toAddress;

        public List<string> GetFormattings() => formattings;
    }
}