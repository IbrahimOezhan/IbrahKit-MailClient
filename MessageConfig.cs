using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailClient
{
    internal class MessageConfig
    {
        [JsonInclude]
        private MessageContentConfig contentConfig = new();

        [JsonInclude]
        private MessageRecepientsConfig recipientsConfig = new();

        public async Task Run()
        {
            await contentConfig.Run();

            recipientsConfig.Run(contentConfig);
        }

        public MessageContentConfig Content() => contentConfig;

        public List<MessageRecepientConfig> GetRecipients() => recipientsConfig.GetRecepientConfigs();

        public static MessageConfig Get(string[] args)
        {
            MessageConfig? msgConfig;

            if (args.Length > 0)
            {
                string json;

                switch (args[0])
                {
                    case "path":

                        if (args.Length != 2)
                        {
                            throw new ArgumentException("Invalid arguments for path command");
                        }

                        if (!File.Exists(args[1]))
                        {
                            throw new ArgumentException("Path doesnt exist");
                        }

                        json = File.ReadAllText(args[1]);

                        break;

                    default:

                        throw new ArgumentException(args[0] + " is an invald argument");
                }

                msgConfig = JsonSerializer.Deserialize<MessageConfig>(json, Utilities.GetJsonOptions());

                if (msgConfig == null) throw new NullReferenceException("Message Comfig is null");

                msgConfig.Content().ConvertURLToHTML();
            }
            else
            {
                msgConfig = new MessageConfig();

                msgConfig.Run().GetAwaiter().GetResult();
            }

            return msgConfig;
        }
    }
}