namespace IbrahKit_MailClient.Exceptions
{
    internal class FileEmptyException : Exception
    {
        public FileEmptyException()
        {
        }

        public FileEmptyException(string? message) : base(message)
        {
        }
    }
}
