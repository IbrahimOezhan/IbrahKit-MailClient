namespace MailClient
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

            try
            {
                Utilities.WriteLine(MailClient.Run(args), ConsoleColor.Green);
            }
            catch(Exception e)
            {
                Utilities.WriteLine(Utilities.FormattedException(e), ConsoleColor.Red);
            }

            Console.WriteLine("Press anything to exit");

            Console.Read();

            Console.WriteLine();

            mutex.ReleaseMutex();
        }
    }
}