namespace MailClient.code.Toolkit.Utilities
{
    public static class StringUtilities
    {
        /// <summary>
        /// Parses the incomming parameters
        /// </summary>
        /// <returns>An empty string if successfull</returns>
        public static bool IsNullEmptyWhite(string text) => string.IsNullOrEmpty(text.Trim()) || string.IsNullOrWhiteSpace(text.Trim());
    }
}
