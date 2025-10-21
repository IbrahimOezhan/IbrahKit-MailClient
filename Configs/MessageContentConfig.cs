using System.Text.Json.Serialization;

namespace MailClient.Configs
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

            body = httpClient.GetStringAsync(body).GetAwaiter().GetResult();
        }

        public bool Valid()
        {
            if(subject == string.Empty)
            {
                Utilities.WriteLine("Subject is empty", ConsoleColor.Red);

                return false;
            }

            if(body == string.Empty)
            {
                Utilities.WriteLine("Body is empty", ConsoleColor.Red);

                return false;
            }

            return true;
        }
    }
}
