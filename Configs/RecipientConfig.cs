using IbrahKit_CLI.Exceptions;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace IbrahKit_MailClient.Configs
{
    internal class RecipientConfig
    {
        private const string regex = "{\\d}";

        [JsonInclude]
        private string toAddress = string.Empty;

        [JsonInclude]
        private List<string> formattings = new();

        public RecipientConfig()
        {

        }

        public RecipientConfig(string adress, List<string> formattings)
        {
            toAddress = adress;
            this.formattings = formattings;
        }

        public bool Validate(SourceConfig contentConfig)
        {
            bool result = true;

            int placeholderAmount = Regex.Count(contentConfig.GetBody(), regex);

            if (toAddress == string.Empty)
            {
                throw new CommandExecutionException($"The recepients address is empty.");
            }

            if (!MailAddress.TryCreate(GetAddress(), out var _))
            {
                throw new CommandExecutionException($"{GetAddress()} is not a valid mail address.");
            }

            int placeholderExpected = GetFormattings().Count;

            if (placeholderExpected != placeholderAmount)
            {
                throw new CommandExecutionException($"The Source Config expects {placeholderExpected} values for placeholders but the RecepientConfig provides {placeholderAmount} values.");
            }

            return result;
        }

        public string GetAddress() => toAddress;

        public List<string> GetFormattings() => formattings;
    }
}