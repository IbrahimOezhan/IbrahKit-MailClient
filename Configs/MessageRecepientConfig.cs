using MailClient.Toolkit.Utilities;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient.Configs
{
    internal class MessageRecepientConfig
    {
        private const string regex = "{\\d}";

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

        public bool Valid(MessageContentConfig contentConfig)
        {
            bool result = true;

            int placeholderAmount = Regex.Count(contentConfig.GetBody(), regex);

            if (toAddress == string.Empty)
            {
                MainUtilities.WriteLine("Address is empty", ConsoleColor.Red);

                result = false;
            }

            if (!MailAddress.TryCreate(GetAdress(), out var _))
            {
                MainUtilities.WriteLine(GetAdress() + " is not a valid mail address.", ConsoleColor.Red);

                result = false;
            }

            int placeholderExpected = GetFormattings().Count;

            if (placeholderExpected != placeholderAmount)
            {
                MainUtilities.WriteLine($"The body contains {placeholderAmount} placeholders but the JSON only provides {placeholderExpected}", ConsoleColor.Red);

                result = false;
            }

            return result;
        }

        public string GetAdress() => toAddress;

        public List<string> GetFormattings() => formattings;
    }
}