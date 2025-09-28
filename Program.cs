namespace MailClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MailClient.Run(args));

            Console.WriteLine("Press anything to exit");

            Console.ReadKey();
        }
    }
}