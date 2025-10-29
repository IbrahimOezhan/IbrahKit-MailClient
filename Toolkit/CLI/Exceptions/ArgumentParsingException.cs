namespace MailClient.Toolkit.CLI.Exceptions
{
    internal class ArgumentParsingException : Exception
    {
        public ArgumentParsingException()
        {
        }

        public ArgumentParsingException(string? message) : base(message)
        {
        }
    }
}
