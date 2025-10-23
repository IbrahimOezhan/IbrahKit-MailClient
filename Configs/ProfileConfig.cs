using MailClient.Exceptions;
using MailClient.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MailClient.Configs
{
    internal class ProfileConfig
    {
        public const string FOLDER = "PROFILES\\";

        [JsonInclude]
        private string profileName;

        [JsonInclude]
        private History.History history = new();

        public ProfileConfig()
        {

        }

        public ProfileConfig(string name)
        {
            profileName = name;
        }

        public string ProfileName() => profileName;

        public History.History GetHistory()
        {
            return history;
        }

        public void Save()
        {
            using StreamWriter sw = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MailClient.FOLDER, FOLDER,profileName + ".json"));

            sw.Write(JsonSerializer.Serialize(this, MainUtilities.GetJsonOptions()));
        }

        private static string GetProfileDirectory()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MailClient.FOLDER, FOLDER);

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

        public static List<string> GetAllConfigs()
        {
            string[] files = Directory.GetFiles(GetProfileDirectory());

            return files.ToList();
        }

        public static bool TryDelete(string name)
        {
            if(TryGet(name, out ProfileConfig config))
            {
                File.Delete(GetExpectedFilePath(name));

                return true;
            }

            return false;
        }

        private static bool TryGet(string name, out ProfileConfig result)
        {
            string folder = GetProfileDirectory();

            string expectedPath = Path.Combine(folder, name + ".json");

            string? config = GetAllConfigs().Find(x => Path.GetFileNameWithoutExtension(x).Equals(name,StringComparison.OrdinalIgnoreCase));

            if(config == null)
            {
                result = null;
                return false;
            }

            string fileContent = File.ReadAllText(config);

            if (StringUtilities.IsNullEmptyWhite(fileContent))
            {
                result = new(name);

                string json = JsonSerializer.Serialize(result, MainUtilities.GetJsonOptions());

                using StreamWriter sw = new(config);

                sw.Write(json);

                return true;
            }

            ProfileConfig? deserialized = JsonSerializer.Deserialize<ProfileConfig>(fileContent, MainUtilities.GetJsonOptions());

            if(deserialized == null)
            {
                result = null;
                return false;
            }

            result = deserialized;

            return true;
        }

        public static bool TryCreate(string name, out ProfileConfig created)
        {
            if(TryGet(name, out ProfileConfig _))
            {
                created = null;
                return false;
            }

            using StreamWriter writer = new(GetExpectedFilePath(name));

            created = new(name);

            string json = JsonSerializer.Serialize(created, MainUtilities.GetJsonOptions());

            writer.Write(json);

            return true;
        }
    }
}
