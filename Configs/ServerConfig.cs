using MailClient.Exceptions;
using MailClient.Utilities;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailClient.Configs
{
    internal class ServerConfig
    {
        [JsonInclude]
        private string fromAddress = string.Empty;

        [JsonInclude]
        private string password = string.Empty;

        [JsonInclude]
        private string smtpServer = string.Empty;

        [JsonInclude]
        private int port = 0;

        public string From() => fromAddress;

        public string Password() => password;

        public string SMTP() => smtpServer;

        public int Port() => port;

        public static ServerConfig Get(string path)
        {
            ServerConfig? config;

            if (!File.Exists(path))
            {
                throw new InvalidConfigException();
            }

            string fileContent = File.ReadAllText(path);

            if (StringUtilities.IsNullEmptyWhite(fileContent) || fileContent == null)
            {
                throw new InvalidConfigException();
            }

            try
            {
                config = JsonSerializer.Deserialize<ServerConfig>(fileContent, MainUtilities.GetJsonOptions());
            }
            catch
            {
                throw new InvalidConfigException();
            }

            if (config == null)
            {
                throw new InvalidConfigException();
            }

            return config;
        }
    }
}