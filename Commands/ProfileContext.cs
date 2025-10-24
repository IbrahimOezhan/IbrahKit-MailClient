namespace MailClient.Commands
{
    internal class ProfileContext
    {
        private string? profileName = null;

        private Mode mode;

        public void SetProfileName(string name)
        {
            this.profileName = name;
        }

        public void SetMode(Mode mode)
        {
            this.mode = mode;
        }

        public string GetName()
        {
            return profileName;
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
