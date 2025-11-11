using IbrahKit_CLI.Exceptions;
using System.Text.Json.Serialization;
using Utilities;

namespace IbrahKit_MailClient.Configs
{
    internal class RecipientsConfig
    {
        [JsonInclude]
        private List<RecipientConfig> recipients = new();

        public static RecipientsConfig Get(string path, SourceConfig sourceConfig)
        {
            if (!File.Exists(path))
            {
                throw new CommandExecutionException($"Couldnt find the recipientconfig at {path}");
            }

            string fileContent = File.ReadAllText(path);

            if (!JsonUtilities.TryDeserialize(fileContent, out RecipientsConfig result, out Exception e))
            {
                throw new CommandExecutionException($"Couldnt find or deserialize the recipients config at {path} with error {e.Message}");
            }

            result.Validate(sourceConfig);

            return result;
        }

        public void Validate(SourceConfig contentConfig)
        {
            for (int i = 0; i < recipients.Count; i++)
            {
                recipients[i].Validate(contentConfig);
            }
        }

        public List<RecipientConfig> GetRecepientConfigs() => recipients;
    }
}