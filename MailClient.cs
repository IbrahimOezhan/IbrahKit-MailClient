using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace MailClient
{
    internal class MailClient
    {
        const string configFile = "MailClientConfig.json";
        const string historyFile = "MailClientHistory.txt";

        static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public async Task<string> Run()
        {
            ProcessModule? module = Process.GetCurrentProcess().MainModule;

            if (module == null)
            {
                return "Error: Module is null";
            }

            string? moduleProccessDir = Path.GetDirectoryName(module.FileName);

            if (moduleProccessDir == null)
            {
                return "Error: Module proccess dir is null";
            }

            string configPath = Path.Combine(moduleProccessDir, configFile);

            string historyPath = Path.Combine(moduleProccessDir, historyFile);

            string historyContent = string.Empty;

            if (File.Exists(historyPath))
            {
                historyContent = File.ReadAllText(historyPath);
            }

            if (!File.Exists(configPath))
            {
                CreateConfigFile(configPath);

                return "Configure the config file";
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

                return $"Error: Config was corrupt was was regenerated. Please start program again\n{ex.ToString()}\n{ex.Message}";
            }

            if (config == null)
            {
                return "Error: Config is null";
            }

            while(true)
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(config.From())
                };

                while (true)
                {
                    bool isFirstAdress = mail.To.Count == 0;

                    Console.Write("\nAdd recepient or enter nothing to continue: ");

                    string? rec = Console.ReadLine();

                    if (rec == null)
                    {
                        continue;
                    }

                    if (rec.Length == 0)
                    {
                        if (!isFirstAdress)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You have to enter at least one recepient");
                            continue;
                        }
                    }

                    if(historyContent.ToLower().Contains(rec.ToLower()))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Warning: {rec} was already used to send a mail");
                        Console.ResetColor();
                    }

                    if (!MailAddress.TryCreate(rec, out var _))
                    {
                        Console.WriteLine("Not a valid adress");
                        continue;
                    }

                    Console.Write("Added: " + rec);

                    mail.To.Add(rec);
                }

                Console.Write("Enter subject: ");

                string? subject = Console.ReadLine();

                while (subject == null || subject.Length == 0)
                {
                    Console.Write("\nPlease enter a valid subject: ");

                    subject = Console.ReadLine();
                }

                mail.Subject = subject;

                var httpClient = new HttpClient();

                string html = await httpClient.GetStringAsync(config.HtmlSource());

                mail.Body = html;

                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new(config.SMTP(), config.Port())
                {
                    Credentials = new NetworkCredential(config.From(), config.Password()),
                    EnableSsl = true
                };

                smtpClient.Send(mail);

                using (StreamWriter sw = new(historyPath))
                {
                    sw.WriteLine(historyContent);

                    foreach (MailAddress? item in mail.To)
                    {
                        sw.WriteLine(item.ToString() + " " + DateTime.Now);
                    }
                }

                Console.Write("Success. Send another Message? Yes (Y) No (Everything else): ");

                ConsoleKeyInfo input = Console.ReadKey();

                if(input.KeyChar.ToString().ToLower() != "y")
                {
                    break;
                }
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
    }
}
