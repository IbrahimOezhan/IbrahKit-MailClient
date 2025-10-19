using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient
{
    internal class MessageRecepientsConfig
    {
        [JsonInclude]
        private List<MessageRecepientConfig> to = new();

        private void AddRecpient(string recepient, int count)
        {
            if (ValidateFormat(recepient))
            {
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
        }

        private static bool ValidateFormat(string mail)
        {
            if (MailAddress.TryCreate(mail, out var _)) return true;

            Console.WriteLine("Not a valid adress");

            return false;
        }

        public List<MessageRecepientConfig> GetRecepientConfigs() => to;

        public void Run(MessageContentConfig contentConfig)
        {
            string regex = "{\\d}";

            int placeholderAmount = Regex.Count(contentConfig.Body(), regex);

            Console.WriteLine(placeholderAmount + " placeholders found\nEnter recepient");

            string? rec = Utilities.ForceInput("You have to enter at least one recepient");

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
    }
}
