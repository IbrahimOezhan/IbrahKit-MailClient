namespace MailClient.Toolkit.CLI
{
    internal abstract class CommandBase
    {
        public abstract string GetCommand();

        public abstract string Execute();

        public abstract string Parse();
    }
}
