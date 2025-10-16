using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace MailClient
{
    internal static class MailClientUtilities
    {
        public const string configFile = "MailClientServerConfig.json";
        public const string historyFile = "MailClientHistory.csv";

        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public static void SaveHistory(StringBuilder history)
        {
            using StreamWriter sw = new(GetHistoryPath());

            sw.Write(history);
        }

        private static string GetHistoryPath()
        {
            return Path.Combine(GetModuleDir(), historyFile);
        }

        public static string GetHistory()
        {
            string historyPath = GetHistoryPath();

            string historyContent = string.Empty;

            if (File.Exists(historyPath))
            {
                historyContent = File.ReadAllText(historyPath);
            }

            return historyContent;
        }

        public static string GetModuleDir()
        {
            ProcessModule? module = Process.GetCurrentProcess().MainModule ?? throw new NullReferenceException("Error: Module is null");

            string? moduleProccessDir = Path.GetDirectoryName(module.FileName) ?? throw new Exception("Error: Module proccess dir is null");

            return moduleProccessDir;
        }

        public static string ForceInput(string errorMsg)
        {
            string? input = Console.ReadLine();

            while (input == null || input == string.Empty || input.Length == 0)
            {
                Console.WriteLine(errorMsg);

                input = Console.ReadLine();
            }

            return input;
        }

        public static string FormattedException(Exception exception)
        {
            return exception.ToString() + "\n" + exception.Message;
        }

        public static JsonSerializerOptions GetJsonOptions() => options;
    }
}