using IbrahKit_CLI.Exceptions;

using MailClient.Utilities;
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

                throw new CommandExecutionException($"The recepients adress is empty");
            }

            if (!MailAddress.TryCreate(GetAdress(), out var _))
            {
                throw new CommandExecutionException($"{GetAdress()} is not a valid mail address.");
            }

            int placeholderExpected = GetFormattings().Count;

            if (placeholderExpected != placeholderAmount)
            {
                throw new CommandExecutionException($"The body contains {placeholderAmount} placeholders but the JSON only provides {placeholderExpected}");
            }

            return result;
        }

        public string GetAdress() => toAddress;

        public List<string> GetFormattings() => formattings;
    }
}