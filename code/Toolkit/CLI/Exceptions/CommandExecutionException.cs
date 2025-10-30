namespace MailClient.code.Toolkit.CLI.Exceptions
{
    internal class CommandExecutionException : Exception
    {
        public CommandExecutionException()
        {
        }

        public CommandExecutionException(string? message) : base(message)
        {
        }
    }
}
