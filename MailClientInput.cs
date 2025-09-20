using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MailClient
{
    internal class MailClientInput
    {
        [JsonInclude]
        private string subject;

        [JsonInclude]
        private string body;

        [JsonInclude]
        private List<To> to = new();

        public class To
        {
            [JsonInclude]
            private string adress;

            [JsonInclude]
            private List<string> formattings = new();

            public To()
            {

            }

            public To(string adress, List<string> formattings)
            {
                this.adress = adress;
                this.formattings = formattings;
            }

            public string GetAdress() => adress;

            public List<string> GetFormattings() => formattings;
        }

        public async Task Run()
        {
            string historyContent = MailClient.GetHistory();

            Console.WriteLine("Enter subject");

            subject = ForceInput("Input cannot be empty");

            Console.WriteLine("Enter body html source url");

            string url = ForceInput("Input cannot be empty");

            var httpClient = new HttpClient();

            string html = await httpClient.GetStringAsync(url);

            body = html;

            string regex = "{\\d}";

            int count = Regex.Count(html, regex);

            Console.WriteLine(count + " placeholders found");

            Console.WriteLine("Enter recepient");

            string rec = ForceInput("You have to enter at least one recepient");

            AddMail(rec, historyContent, to, count);

            while (true)
            {
                Console.WriteLine("Add more recepients or submit empty line to continue");

                rec = Console.ReadLine();

                if (rec.Length == 0 || rec == null)
                {
                    break;
                }

                AddMail(rec, historyContent, to, count);
            }
        }

        private static void AddMail(string rec, string historyContent, List<To> to, int count)
        {
            if (ValidateMail(rec, historyContent))
            {
                Console.WriteLine("Added: " + rec);

                List<string> formattings = new();

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine("Add value for placeholder " + i);

                    formattings.Add(Console.ReadLine());
                }

                to.Add(new(rec, formattings));
            }
        }

        private static bool ValidateMail(string mail, string history)
        {
            if (!MailAddress.TryCreate(mail, out var _))
            {
                Console.WriteLine("Not a valid adress");

                return false;
            }

            if (history.ToLower().Contains(mail.ToLower()))
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Warning: {mail} was already used to send a mail");

                Console.ResetColor();
            }

            return true;
        }

        public static string ForceInput(string errorMsg)
        {
            string input = Console.ReadLine();

            while (input == null || input == string.Empty || input.Length == 0)
            {
                Console.WriteLine(errorMsg);

                input = Console.ReadLine();
            }

            return input;
        }

        public string Subject() => subject;

        public string Body() => body;

        public List<To> GetTos() => to;
    }
}