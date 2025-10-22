using MailClient.Utilities;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient.Configs
{
    internal class MessageRecepientsConfig
    {
        private const string regex = "{\\d}";

        [JsonInclude]
        private List<MessageRecepientConfig> recipients = new();

        public bool Valid(MessageContentConfig contentConfig)
        {
            int placeholderAmount = Regex.Count(contentConfig.Body(), regex);

            bool result = true;

            for (int i = 0; i < recipients.Count; i++)
            {
                if (!MailAddress.TryCreate(recipients[i].GetAdress(), out var _))
                {
                    MainUtilities.WriteLine(recipients[i].GetAdress() + " is not a valid mail address.", ConsoleColor.Red);

                    result = false;
                }

                int placeholderExpected = recipients[i].GetFormattings().Count;

                if (placeholderExpected != placeholderAmount)
                {
                    MainUtilities.WriteLine($"The body contains {placeholderAmount} placeholders but the JSON only provides {placeholderExpected}", ConsoleColor.Red);

                    result = false;
                }
            }

            for (int i = 0; i < recipients.Count; i++)
            {
                if (!recipients[i].Valid()) result = false;
            }

            return result;
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => recipients;
    }
}