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

        public bool Valid()
        {
            if (fromAddress == string.Empty)
            {
                return false;
            }

            if (!MailAddress.TryCreate(fromAddress, out _))
            {
                return false;
            }

            if (smtpServer == string.Empty)
            {
                return false;
            }

            return true;
        }

        public static ServerConfig Get(string path = "")
        {
            if (!File.Exists(path)) throw new FileNotFoundException("Config file not found", path);

            string fileContent = File.ReadAllText(path);

            if (StringUtilities.IsNullEmptyWhite(fileContent)) throw new FileEmptyException(path + " is empty");

            ServerConfig? config = JsonSerializer.Deserialize<ServerConfig>(fileContent, MainUtilities.GetJsonOptions());

            if (config == null) throw new NullReferenceException();

            if (!config.Valid()) throw new ConfigInvalidException();

            return config;
        }
    }
}