namespace MailClient.Commands
{
    internal abstract class Command
    {
        protected string[] args;

        public Command(string[] args)
        {
            this.args = args;
        }

        public abstract string Run();

        public abstract string CommandName();
    }
}
