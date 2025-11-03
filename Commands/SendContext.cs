
using IbrahKit_CLI;
using IbrahKit_CLI.Exceptions;
using IbrahKit_MailClient.Configs;

namespace IbrahKit_MailClient.Commands
{
    internal class SendContext : Context
    {
        private string server = string.Empty;

        private string message = string.Empty;

        private string profile = string.Empty;

        private bool include = false;

        private bool skip = false;

        private MessageContentConfig.MessageContentBodyMode bodyMode = MessageContentConfig.MessageContentBodyMode.URL;

        public void Skip()
        {
            if (include)
            {
                throw new ArgumentParsingException($"The command already defined --includeDuplicates which collides with --skipDuplicates");
            }
            skip = true;
        }

        public void Include()
        {
            if (skip)
            {
                throw new ArgumentParsingException($"The command already defined --skipDuplicates which collides with --includeDuplicates");
            }
            include = true;
        }

        public void SetServer(string server)
        {
            this.server = server;
        }

        public void SetMessage(string message)
        {
            this.message = message;
        }

        public void SetProfile(string profile)
        {
            this.profile = profile;
        }

        public void SetBodyMode(MessageContentConfig.MessageContentBodyMode bodyMode)
        {
            this.bodyMode = bodyMode;
        }

        public bool GetSkip() => skip;
        public bool GetInclude() => include;

        public ProfileConfig GetProfile()
        {
            if (profile == string.Empty)
            {
                throw new CommandExecutionException($"The provided value of the profile parameter is null");
            }

            if (ProfileConfig.TryGet(profile, out ProfileConfig result))
            {
                return result;
            }

            ProfileCommand command = new([]);

            throw new CommandExecutionException($"The operation was cancelled because the profile {profile} does not exist. Use \"{command.GetData().name} {ProfileContext.Mode.CREATE} <name>\" to create one");
        }

        public ServerConfig GetServerConfig()
        {
            if (server == string.Empty)
            {
                throw new CommandExecutionException($"The provided value of the server parameter is null");
            }

            return ServerConfig.Get(server);
        }

        public MessageConfig GetMessageConfig()
        {
            if (message == string.Empty)
            {
                throw new CommandExecutionException($"The provided value of the message parameter is null");
            }

            return MessageConfig.Get(message, bodyMode);
        }
    }
}