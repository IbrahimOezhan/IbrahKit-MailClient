using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient
{
    internal class MailClientMessageConfig
    {
        [JsonInclude]
        private string subject;

        [JsonInclude]
        private string body;

        [JsonInclude]
        private List<MailClientRecepientConfig> to = new();

        public async Task Run()
        {
            Console.WriteLine("Enter subject");

            subject = MailClientUtilities.ForceInput("Input cannot be empty");

            Console.WriteLine("Enter body html source url");

            string url = MailClientUtilities.ForceInput("Input cannot be empty");

            var httpClient = new HttpClient();

            string html = await httpClient.GetStringAsync(url);

            body = html;

            string regex = "{\\d}";

            int placeholderAmount = Regex.Count(html, regex);

            Console.WriteLine(placeholderAmount + " placeholders found\nEnter recepient");

            string? rec = MailClientUtilities.ForceInput("You have to enter at least one recepient");

            AddMail(rec, to, placeholderAmount);

            while (true)
            {
                Console.WriteLine("Add more recepients or submit empty line to continue");

                rec = Console.ReadLine();

                if (rec == null || rec.Length == 0)
                {
                    break;
                }

                AddMail(rec, to, placeholderAmount);
            }
        }

        private static void AddMail(string rec, List<MailClientRecepientConfig> to, int count)
        {
            if (ValidateFormat(rec))
            {
                Console.WriteLine("Added: " + rec);

                List<string> formattings = new();

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine("Add value for placeholder " + i);

                    string? formatInput = Console.ReadLine();

                    formattings.Add(formatInput?? "");
                }

                to.Add(new(rec, formattings));
            }
        }

        private static bool ValidateFormat(string mail)
        {
            if (MailAddress.TryCreate(mail, out var _)) return true;

            Console.WriteLine("Not a valid adress");

            return false;
        }

        public bool ValidateHistory(string history)
        {
            bool result = true;

            for (int i = 0; i < to.Count; i++)
            {
                if (!history.Contains(to[i].GetAdress(),StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Warning: {to[i].GetAdress()} was already used to send a mail");

                Console.ResetColor();

                result = false;
            }

            return result;
        }

        public string Subject() => subject;

        public string Body() => body;

        public List<MailClientRecepientConfig> GetTos() => to;
    }
}