namespace MailClient
{
    internal class Program
    {
        private const string programID = "IbrahKit.MailClient";

        static void Main(string[] args)
        {
            Mutex mutex = new(true, $"Global\\{programID}", out bool createdNew);

            if (!createdNew)
            {
                Console.WriteLine("App is already running.");
                return;
            }

            Console.WriteLine(MailClient.Run(args));

            Console.WriteLine("Press anything to exit");

            Console.ReadKey();

            mutex.ReleaseMutex();
        }
    }
}