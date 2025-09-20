using System.Text.Json.Serialization;

namespace MailClient
{
    internal class MailClientConfig
    {
        [JsonInclude]
        private string fromAdress;

        [JsonInclude]
        private string password;

        [JsonInclude]
        private string smtpServer;

        [JsonInclude]
        private int port;

        [JsonInclude]
        private string htmlSource;

        public string From() => fromAdress;

        public string Password() => password;

        public string SMTP() => smtpServer;

        public int Port() => port;

        public string HtmlSource() => htmlSource;
    }
}
