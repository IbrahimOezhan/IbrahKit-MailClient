using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailClient
{
    internal class MailClientServerConfig
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

        private static void CreateConfigFile(string path)
        {
            MailClientServerConfig config = new();

            string defaultJson = JsonSerializer.Serialize(config, MailClientUtilities.GetJsonOptions());

            using (StreamWriter sw = new(path))
            {
                sw.Write(defaultJson);
            }
        }

        public static MailClientServerConfig Get()
        {
            string configPath = Path.Combine(MailClientUtilities.GetModuleDir(), MailClientUtilities.configFile);

            if (!File.Exists(configPath))
            {
                CreateConfigFile(configPath);

                throw new FileNotFoundException("Configure the config file");
            }

            string fileContent = File.ReadAllText(configPath);

            MailClientServerConfig? config;

            try
            {
                config = JsonSerializer.Deserialize<MailClientServerConfig>(fileContent);
            }
            catch (Exception ex)
            {
                CreateConfigFile(configPath);

                throw new JsonException($"Error: Config was corrupt was was regenerated. Please start program again\n{ex.ToString()}\n{ex.Message}");
            }

            if (config == null)
            {
                throw new NullReferenceException("Error: Config is null");
            }

            return config;
        }
    }
}
