using MailClient.Utilities;

namespace MailClient.Main
{
    internal class Program
    {
        private const string programID = "IbrahKit.MailClient";

        static void Main(string[] args)
        {
            Console.Clear();

            Console.WriteLine("Launching MailClient\n");

            Mutex mutex = new(true, $"Global\\{programID}", out bool createdNew);

            if (!createdNew)
            {
                Console.WriteLine("App is already running.");

                return;
            }

            MailClient client = new();

            client.Run(args);

            Console.WriteLine("Press anything to exit");

            Console.Read();

            Console.WriteLine();

            mutex.ReleaseMutex();
        }
    }
}