using MailClient.Exceptions;
using MailClient.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public static ProfileConfig Get(string name)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MailClient.FOLDER, FOLDER);

            string expectedPath = Path.Combine(folder, name + ".json");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

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
                    bool value = MainUtilities.InputYesNo('y', 'n', "Config file found but empty. Generate a new config?", "Invalid Input");

                    if (value)
                    {
                        return CreateNew(expectedPath);
                    }
                    else
                    {
                        throw new ConfigInvalidException();
                    }
                }
                else
                {
                    ProfileConfig? config = JsonSerializer.Deserialize<ProfileConfig>(fileContent, MainUtilities.GetJsonOptions());

                    if (config == null) throw new NullReferenceException();

                    return config;
                }
            }
            else
            {
                bool res = MainUtilities.InputYesNo('y', 'n', $"Config file with name {name} not found . Generate a new config?", "Invalid Input");

                if (res)
                {
                    return CreateNew(expectedPath);
                }
                else
                {
                    throw new ConfigInvalidException();
                }
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
