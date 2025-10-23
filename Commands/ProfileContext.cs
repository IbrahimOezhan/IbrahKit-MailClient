namespace MailClient.Commands
{
    internal class ProfileContext
    {
        private string? name = null;

        private Mode mode;

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetMode(Mode mode)
        {
            this.mode = mode;
        }

        public string GetName()
        {
            return name;
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
