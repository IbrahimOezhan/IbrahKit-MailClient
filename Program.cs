using IbrahKit_CLI;

namespace IbrahKit_MailClient
{
    internal class Program
    {
        private const string programID = "Global\\IbrahKit.MailClient";

        static void Main(string[] args)
        {
            Console.Clear();

            Console.WriteLine("Launching MailClient\n");

            Mutex mutex = new(true, programID, out bool createdNew);

            if (!createdNew)
            {
                Console.WriteLine("App is already running.");

                return;
            }

            CLI client = new();

            try
            {
                client.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press anything to exit");

            Console.Read();

            Console.WriteLine();

            mutex.ReleaseMutex();
        }
    }
}