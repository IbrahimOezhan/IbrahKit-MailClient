using MailClient.Configs;
using MailClient.Exceptions;
using MailClient.Utilities;

namespace MailClient.Commands
{
    internal class SendContext
    {
        private string? server = null;
        private string? message = null;
        private string? profile = null;

        private MessageContentConfig.MessageContentBodyMode bodyMode = MessageContentConfig.MessageContentBodyMode.URL;

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

        public ProfileConfig GetProfile()
        {
            if (profile == null)
            {
                throw new InvalidConfigException();
            }

            if (!ProfileConfig.TryGet(profile, out ProfileConfig result))
            {
                if (MainUtilities.InputYesNo('Y', 'N', "Profile does not exist. Create a new one?", "Must make a selection."))
                {
                    if (!ProfileConfig.TryCreate(profile, out result))
                    {
                        throw new InvalidConfigException();
                    }
                }
                else
                {
                    throw new InvalidConfigException();
                }
            }

            return result;
        }

        public ServerConfig GetServerConfig()
        {
            if (server == null)
            {
                throw new InvalidConfigException();
            }

            return ServerConfig.Get(server);
        }

        public MessageConfig GetMessageConfig()
        {
            return MessageConfig.Get(message, bodyMode);
        }
    }
}
