namespace MailClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MailClient mailClient = new();

                string result = mailClient.Run().GetAwaiter().GetResult();

                Console.WriteLine(result);
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
