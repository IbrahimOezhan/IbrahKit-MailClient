namespace MailClient.Commands
{
    internal abstract class Command(string[] args)
    {
        protected string[] args = args;

        public abstract string Run();

        protected abstract string Execute();

        public abstract string CommandName();
    }
}