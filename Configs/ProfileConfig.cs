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

        public static ProfileConfig Get(string name)
        {
            string folder = GetProfileDirectory();

            string expectedPath = Path.Combine(folder, name + ".json");

            string[] files = Directory.GetFiles(folder);

            bool found = false;

            int foundIndex = -1;

            Console.WriteLine($"Profiles in {folder}:");

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] == expectedPath)
                {
                    found = true;
                    foundIndex = i;
                }

                Console.WriteLine($"{i}: {files[i]}");
            }

            if (found)
            {
                string fileContent = File.ReadAllText(files[foundIndex]);

                if (StringUtilities.IsNullEmptyWhite(fileContent))
                {
                    bool result = MainUtilities.InputYesNo('y', 'n', "Config file found but empty. Generate a new config?", "Invalid Input");

                    return Decision(expectedPath, result);
                }
                else
                {
                    ProfileConfig? config = JsonSerializer.Deserialize<ProfileConfig>(fileContent, MainUtilities.GetJsonOptions());

                    return config ?? throw new NullReferenceException();
                }
            }
            else
            {
                bool result = MainUtilities.InputYesNo('y', 'n', $"Config file with name {name} not found . Generate a new config?", "Invalid Input");

                return Decision(expectedPath, result);
            }
        }

        private static ProfileConfig Decision(string path, bool dec)
        {
            if (dec)
            {
                return CreateNew(path);
            }
            else
            {
                throw new ConfigInvalidException();
            }
        }

        private static ProfileConfig CreateNew(string name)
        {
            using StreamWriter writer = new(name);

            ProfileConfig config = new(Path.GetFileNameWithoutExtension(name));

            string json = JsonSerializer.Serialize(config, MainUtilities.GetJsonOptions());

            writer.Write(json);

            return config;
        }
    }
}
