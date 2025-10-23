using MailClient.Commands;

namespace MailClient.Main
{
    internal class MailClient
    {
        public const string FOLDER = "IbrahKit";

        public void Run(string[] args)
        {
            string res = "";

            CommandHandler handler = new();

            do
            {
                res = handler.Run(args);

                Console.WriteLine(res);
            }
            while (res != null);
        }
    }
}
