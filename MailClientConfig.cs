using System.Text.Json.Serialization;

namespace MailClient
{
    internal class MailClientConfig
    {
        [JsonInclude]
        private string fromAdress;

        [JsonInclude]
        private string smtpServer;

        [JsonInclude]
        private int port;

        [JsonInclude]
        private string password;

        [JsonInclude]
        private string htmlSource;

        public string From() => fromAdress;

        public string SMTP() => smtpServer;

        public string Password() => password;

        public string HtmlSource() => htmlSource;

        public int Port() => port;
    }
}
