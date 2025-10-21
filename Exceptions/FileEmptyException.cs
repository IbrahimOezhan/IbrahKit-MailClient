namespace MailClient.Exceptions
{
    [Serializable]
    internal class FileEmptyException : Exception
    {
        public FileEmptyException()
        {

        }

        public FileEmptyException(string message) : base(message)
        {

        }

        public FileEmptyException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
