using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient.Configs
{
    internal class MessageRecepientsConfig
    {
        private const string regex = "{\\d}";

        [JsonInclude]
        private List<MessageRecepientConfig> to = new();

        public bool Valid(MessageContentConfig contentConfig)
        {
            int placeholderAmount = Regex.Count(contentConfig.Body(), regex);

            bool result = true;

            for (int i = 0; i < to.Count; i++)
            {
                if (!MailAddress.TryCreate(to[i].GetAdress(), out var _))
                {
                    Utilities.WriteLine(to[i].GetAdress() + " is not a valid mail address.", ConsoleColor.Red);

                    result = false;
                }

                int placeholderExpected = to[i].GetFormattings().Count;

                if (placeholderExpected != placeholderAmount)
                {
                    Utilities.WriteLine($"The body contains {placeholderAmount} placeholders but the JSON only provides {placeholderExpected}", ConsoleColor.Red);

                    result = false;
                }
            }

            for (int i = 0; i < to.Count; i++)
            {
                if (!to[i].Valid()) result = false;
            }

            return result;
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => to;
    }
}