using System.Text.Json;

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
                Console.WriteLine(ex.ToString());

                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press anything to exit");

            Console.ReadKey();
        }
    }
}