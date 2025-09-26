using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MailClient
{
    internal static class MailClientUtilities
    {
        public const string configFile = "MailClientConfig.json";
        public const string historyFile = "MailClientHistory.txt";

        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public static string GetHistoryPath()
        {
            return Path.Combine(GetModuleDir(), historyFile);
        }

        public static JsonSerializerOptions GetJsonOptions() => options;

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
    }
}
