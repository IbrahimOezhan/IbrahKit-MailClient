using IbrahKit_CLI;
using IbrahKit_MailClient.Utilities;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IbrahKit_MailClient.Configs
{
    internal class ProfileConfig
    {
        public const string FOLDER = "PROFILES\\";

        [JsonInclude]
        private string profileName = string.Empty;

        [JsonInclude]
        private History.History history = new();

        public ProfileConfig()
        {

        }

        public ProfileConfig(string name)
        {
            profileName = name;
        }

        public void SaveConfig()
        {
            using StreamWriter sw = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CLI.FOLDER, FOLDER, profileName + ".json"));

            sw.Write(JsonSerializer.Serialize(this, MainUtilities.GetJsonOptions()));
        }

        public string GetProfileName() => profileName;

        public History.History GetHistory() => history;

        private static string GetProfileDirectory()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CLI.FOLDER, FOLDER);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        private static string GetExpectedFilePath(string name)
        {
            return Path.Combine(GetProfileDirectory(), name + ".json");
        }

        public static List<ProfileConfig> GetConfigs(StringBuilder? log = null)
        {
            List<string> configFiles = GetConfigFiles();

            List<ProfileConfig> configs = new();

            for (int i = 0; i < configFiles.Count; i++)
            {
                if (TryGet(configFiles[i], out ProfileConfig result, log))
                {
                    configs.Add(result);
                }
            }

            return configs;
        }

        public static List<string> GetConfigFiles()
        {
            string[] files = Directory.GetFiles(GetProfileDirectory());

            return [.. files];
        }

        public static bool TryCreate(string name, out ProfileConfig created, StringBuilder? log = null)
        {
            created = new();

            if (TryGet(name, out ProfileConfig _, log))
            {
                return false;
            }

            created = new(name);

            string json = JsonSerializer.Serialize(created, MainUtilities.GetJsonOptions());

            using StreamWriter writer = new(GetExpectedFilePath(name));

            writer.Write(json);

            return true;
        }

        public static bool TryDelete(string name, StringBuilder? log = null)
        {
            if (!TryGet(name, out _, log))
            {
                return false;
            }

            File.Delete(GetExpectedFilePath(name));

            return true;
        }

        public static bool TryGet(string name, out ProfileConfig result, StringBuilder? log = null)
        {
            result = new();

            string expectedPath = GetExpectedFilePath(name);

            bool found = GetConfigFiles().Find(x => Path.GetFileNameWithoutExtension(x).Equals(name, StringComparison.InvariantCultureIgnoreCase)) != null;

            if (!found)
            {
                log?.AppendLine($"Profile matching {name} was not found.");

                return false;
            }

            string fileContent = File.ReadAllText(expectedPath);

            if (StringUtilities.IsNullEmptyWhite(fileContent))
            {
                result = new(name);

                string json = JsonSerializer.Serialize(result, MainUtilities.GetJsonOptions());

                using StreamWriter sw = new(expectedPath);

                sw.Write(json);

                log?.AppendLine($"Profile {name} was found but empty. Recreated the json.");

                return true;
            }

            try
            {
                ProfileConfig? jsonResult = JsonSerializer.Deserialize<ProfileConfig>(fileContent, MainUtilities.GetJsonOptions());

                if (jsonResult == null)
                {
                    log?.AppendLine($"Deserialization of json from profile {name} resulted in null value.");

                    return false;
                }

                result = jsonResult;
            }
            catch (Exception e)
            {
                log?.AppendLine($"Exception when deserializing the json from profile. {name} \n {e.ToString()}");

                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return profileName;
        }
    }
}