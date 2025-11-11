using IbrahKit_CLI.Exceptions;
using System.Text.Json.Serialization;
using Utilities;

namespace IbrahKit_MailClient.Configs
{
    internal class RecepientsConfig
    {
        [JsonInclude]
        private List<RecepientConfig> recipients = new();

        public static RecepientsConfig Get(string path, SourceConfig sourceConfig)
        {
            if (!JsonUtilities.TryDeserialize(path, out RecepientsConfig result,out Exception e))
            {
                throw new CommandExecutionException($"Couldnt find or deserialize the source config at {path} with error {e.Message}");
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

        public List<RecepientConfig> GetRecepientConfigs() => recipients;
    }
}