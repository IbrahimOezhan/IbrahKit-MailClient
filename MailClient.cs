using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace MailClient
{
    internal class MailClient
    {
        public static string Run(MailClientInput input)
        {
            StringBuilder historyContent = new(MailClientUtilities.GetHistory());

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

                historyContent.AppendLine(adr.ToString() + " " + DateTime.Now);

                mail.To.Add(adr);

                try
                {
                    smtpClient.Send(mail);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(mail.To);
                    Console.WriteLine(ex.ToString()); 
                }
            }

            using (StreamWriter sw = new(MailClientUtilities.GetHistoryPath()))
            {
                sw.Write(historyContent);
            }

            return "";
        }

        private static void CreateConfigFile(string path)
        {
            MailClientConfig config = new();

            string defaultJson = JsonSerializer.Serialize(config, MailClientUtilities.GetJsonOptions());

            using (StreamWriter sw = new(path))
            {
                sw.Write(defaultJson);
            }
        }

        private static MailClientConfig GetConfig()
        {
            string configPath = Path.Combine(MailClientUtilities.GetModuleDir(), MailClientUtilities.configFile);

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
    }
}
