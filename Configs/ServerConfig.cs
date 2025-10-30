using IbrahKit_CLI.Exceptions;

using MailClient.Utilities;
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
                throw new CommandExecutionException($"ServerConfig at path {path} does not exist.");
            }

            string fileContent = File.ReadAllText(path);

            if (StringUtilities.IsNullEmptyWhite(fileContent) || fileContent == null)
            {
                throw new CommandExecutionException($"The contents of ServerConfig at path {path} are empty.");
            }

            try
            {
                config = JsonSerializer.Deserialize<ServerConfig>(fileContent, MainUtilities.GetJsonOptions());
            }
            catch (Exception e)
            {
                throw new CommandExecutionException($"An error was encountered during the deserialization attempt: {e.Message}");
            }

            if (config == null)
            {
                throw new CommandExecutionException($"The value of the deserialized ServerConfig json at {path} is null.");
            }

            return config;
        }
    }
}