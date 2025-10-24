namespace MailClient.Commands
{
    internal abstract class Command
    {
        protected string[] args;

        public Command()
        {

        }

        public Command(string[] args)
        {
            this.args = args;
        }

        public abstract string Parse();

        public abstract string Execute();

        public abstract string GetCommand();
    }
}