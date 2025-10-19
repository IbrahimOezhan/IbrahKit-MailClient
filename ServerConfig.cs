using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailClient
{
    internal class ServerConfig
    {
        public const string configFile = "ServerConfig.json";

        [JsonInclude]
        private string fromAdress = string.Empty;

        [JsonInclude]
        private string password = string.Empty;

        [JsonInclude]
        private string smtpServer = string.Empty;

        [JsonInclude]
        private int port = 0;

        [JsonInclude]
        private string htmlSource = string.Empty;

        public string From() => fromAdress;

        public string Password() => password;

        public string SMTP() => smtpServer;

        public int Port() => port;

        public string HtmlSource() => htmlSource;

        public static ServerConfig Get()
        {
            string configPath = Path.Combine(Utilities.GetModuleDir(), configFile);

            try
            {
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException("Config file not found", configPath);
                }

                string fileContent = File.ReadAllText(configPath);

                ServerConfig? config = JsonSerializer.Deserialize<ServerConfig>(fileContent);

                return config ?? throw new NullReferenceException("Config is null");
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch
            {
                Console.WriteLine("Recreated Config File");

                ServerConfig config = new();

                string defaultJson = JsonSerializer.Serialize(config, Utilities.GetJsonOptions());

                using StreamWriter sw = new(configPath);

                sw.Write(defaultJson);

                throw;
            }
        }
    }
}
