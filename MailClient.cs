using MailClient.Commands;

namespace MailClient
{
    internal class MailClient
    {
        public const string FOLDER = "IbrahKit";

        public string Run(string[] args)
        {
            string res = "";

            CommandHandler handler = new();

            do
            {
                res = handler.Run(args);
            }
            while (res != null);

            return "";
        }
    }
}
