namespace MailClient.Utilities
{
    public static class StringUtilities
    {
        public static bool IsNullEmptyWhite(string text) => string.IsNullOrEmpty(text.Trim()) || string.IsNullOrWhiteSpace(text.Trim());
    }
}
