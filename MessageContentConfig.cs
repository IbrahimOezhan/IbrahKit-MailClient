using System.Text.Json.Serialization;

namespace MailClient
{
    internal class MessageContentConfig
    {
        [JsonInclude]
        private string subject = string.Empty;

        [JsonInclude]
        private string body = string.Empty;

        public string Subject() => subject;

        public string Body() => body;

        public void ConvertURLToHTML()
        {
            var httpClient = new HttpClient();

            body =  httpClient.GetStringAsync(body).GetAwaiter().GetResult();
        }

        public async Task Run()
        {
            Console.WriteLine("Enter subject");

            subject = Utilities.ForceInput("A subject is required");

            Console.WriteLine("Select body selection mode. Path (1), Adress (2)");

            ConsoleKeyInfo input = Console.ReadKey();

            int inputNumber = input.KeyChar - '0';

            switch (inputNumber)
            {
                case 1:

                    string? pathInput = Utilities.ForceInput("Must enter path");

                    if (!File.Exists(pathInput))
                    {
                        throw new FileNotFoundException();
                    }

                    body = File.ReadAllText(pathInput);

                    break;
                case 2:

                    string? urlInput = Utilities.ForceInput("Must enter URL");

                    var httpClient = new HttpClient();

                    body = await httpClient.GetStringAsync(urlInput);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
