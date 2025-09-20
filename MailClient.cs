using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace MailClient
{
    internal class MailClient
    {
        public const string configFile = "MailClientConfig.json";
        public const string historyFile = "MailClientHistory.txt";

        public static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public static string Run(MailClientInput input)
        {
            StringBuilder historyContent = new(GetHistory());

            MailClientConfig config = GetConfig();

            SmtpClient smtpClient = new(config.SMTP(), config.Port())
            {
                Credentials = new NetworkCredential(config.From(), config.Password()),
                EnableSsl = true
            };

            for (int i = 0; i < input.GetTos().Count; i++)
            {
                object[] format;

                format = input.GetTos()[i].GetFormattings().ToArray();

                MailMessage mail = new()
                {
                    From = new MailAddress(config.From()),

                    Subject = input.Subject(),

                    Body = string.Format(input.Body(), format),

                    IsBodyHtml = true
                };

                string adr = input.GetTos()[i].GetAdress();

                historyContent.Append(adr.ToString() + " " + DateTime.Now);

                mail.To.Add(adr);

                smtpClient.Send(mail);
            }

            using (StreamWriter sw = new(GetHistoryPath()))
            {
                sw.Write(historyContent);
            }

            return "";
        }

        private static void CreateConfigFile(string path)
        {
            MailClientConfig config = new();

            string defaultJson = JsonSerializer.Serialize(config, options);

            using (StreamWriter sw = new(path))
            {
                sw.Write(defaultJson);
            }
        }

        private static MailClientConfig GetConfig()
        {
            string configPath = Path.Combine(GetModuleDir(), configFile);

            if (!File.Exists(configPath))
            {
                CreateConfigFile(configPath);

                throw new Exception("Configure the config file");
            }

            string fileContent = File.ReadAllText(configPath);

            MailClientConfig? config;

            try
            {
                config = JsonSerializer.Deserialize<MailClientConfig>(fileContent);
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

        public static string GetModuleDir()
        {
            ProcessModule? module = Process.GetCurrentProcess().MainModule;

            if (module == null)
            {
                return "Error: Module is null";
            }

            string? moduleProccessDir = Path.GetDirectoryName(module.FileName);

            if (moduleProccessDir == null)
            {
                throw new Exception("Error: Module proccess dir is null");
            }

            return moduleProccessDir;
        }

        private static string GetHistoryPath()
        {
            return Path.Combine(GetModuleDir(), historyFile);
        }

        public static string GetHistory()
        {
            string historyPath = Path.Combine(GetModuleDir(), historyFile);

            string historyContent = string.Empty;

            if (File.Exists(historyPath))
            {
                historyContent = File.ReadAllText(historyPath);
            }

            return historyContent;
        }
    }
}
