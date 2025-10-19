using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient
{
    internal class MessageRecepientsConfig
    {
        const string regex = "{\\d}";

        [JsonInclude]
        private List<MessageRecepientConfig> to = new();

        public void Run(MessageContentConfig contentConfig)
        {
            int placeholderAmount = Regex.Count(contentConfig.Body(), regex);

            string? rec = Utilities.ForceInput(placeholderAmount + " placeholders found\nEnter recepient", "You have to enter at least one recepient");

            AddRecpient(rec, placeholderAmount);

            while (true)
            {
                Console.WriteLine("Add more recepients or submit empty line to continue");

                rec = Console.ReadLine();

                if (rec == null || rec.Length == 0)
                {
                    break;
                }

                AddRecpient(rec, placeholderAmount);
            }
        }

        private void AddRecpient(string recepient, int count)
        {
            if (!ValidateFormat(recepient))
            {
                return;
            }

            Console.WriteLine("Added: " + recepient);

            List<string> formattings = new();

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Add value for placeholder " + i);

                string? formatInput = Console.ReadLine();

                formattings.Add(formatInput ?? string.Empty);
            }

            to.Add(new(recepient, formattings));
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => to;

        private static bool ValidateFormat(string mail)
        {
            if (MailAddress.TryCreate(mail, out var _)) return true;

            Utilities.WriteLine(mail + " is not a valid mail address.", ConsoleColor.Yellow);

            return false;
        }
    }
}