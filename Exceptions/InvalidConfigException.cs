namespace IbrahKit_MailClient.Exceptions
{
    internal class InvalidConfigException : Exception
    {
        public InvalidConfigException()
        {
        }

        public InvalidConfigException(string? message) : base(message)
        {
        }
    }
}
