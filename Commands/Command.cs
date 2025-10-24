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

        public abstract string Run();

        protected abstract string Execute();

        public abstract string CommandName();
    }
}