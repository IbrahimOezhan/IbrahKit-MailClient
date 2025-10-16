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

            using StreamWriter sw = new(path);

            sw.Write(defaultJson);
        }

        public static MailClientServerConfig Get()
        {
            string configPath = Path.Combine(MailClientUtilities.GetModuleDir(), MailClientUtilities.configFile);

            try
            {
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException("Config file not found",configPath);
                }

                string fileContent = File.ReadAllText(configPath);

                MailClientServerConfig? config = JsonSerializer.Deserialize<MailClientServerConfig>(fileContent);

                return config ?? throw new NullReferenceException("Config is null");
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch
            {
                Console.WriteLine("Recreated Config File");

                CreateConfigFile(configPath);               

                throw;
            }
        }
    }
}
