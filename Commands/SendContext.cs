using MailClient.Configs;

namespace MailClient.Commands
{
    internal class SendContext
    {
        private string server = null;
        private string message = null;
        private string profile = null;

        public void SetServer(string wert)
        {

        }

        public void SetMessage(string message)
        {

        }

        public void SetProfile(string profile)
        {

        }

        public ProfileConfig GetProfile()
        {
            throw new NotImplementedException();
        }

        public ServerConfig GetServerConfig()
        {
            throw new NotImplementedException();
        }

        public MessageConfig GetMessageConfig()
        {
            throw new NotImplementedException();
        }
    }
}
