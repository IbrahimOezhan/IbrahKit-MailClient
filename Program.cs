using System.Text.Json;

namespace MailClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MailClientInput input = null;

                switch (args.Length)
                {
                    case 0:
                        input = new MailClientInput();
                        input.Run().GetAwaiter().GetResult();
                        break;
                    case 1:
                        input = JsonSerializer.Deserialize<MailClientInput>(args[0], MailClient.options);
                        break;
                    default:
                        throw new Exception("Cannot pass more than 1 arg");
                }

                Console.WriteLine(MailClient.Run(input));
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