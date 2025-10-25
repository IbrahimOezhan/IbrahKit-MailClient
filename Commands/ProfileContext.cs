using MailClient.Exceptions;
using MailClient.Toolkit.CLI;

namespace MailClient.Commands
{
    internal class ProfileContext : Context
    {
        private string? profile = null;

        private Mode mode;

        public void SetProfile(string profile)
        {
            this.profile = profile;
        }

        public void SetMode(Mode mode)
        {
            this.mode = mode;
        }

        public string GetProfile()
        {
            if (profile == null)
            {
                throw new InvalidConfigException($"The provided value of the profile parameter is null");
            }

            return profile;
        }

        public Mode GetMode()
        {
            return mode;
        }

        internal enum Mode
        {
            LIST,
            CREATE,
            DELETE,
        }
    }
}
