namespace MailClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(MailClient.Run(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(MailClientUtilities.FormattedException(ex));
            }

            Console.WriteLine("Press anything to exit");

            Console.ReadKey();
        }
    }
}