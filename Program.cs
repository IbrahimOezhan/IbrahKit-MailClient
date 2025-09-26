using System.Text.Json;

namespace MailClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(Run(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press anything to exit");

            Console.ReadKey();
        }

        private static string Run(string[] args)
        {
            MailClientInput? input = null;

            switch (args.Length)
            {
                case 0:
                    input = new MailClientInput();
                    input.Run().GetAwaiter().GetResult();
                    break;
                case 1:
                    input = JsonSerializer.Deserialize<MailClientInput>(args[0], MailClientUtilities.GetJsonOptions());
                    break;
                default:
                    throw new Exception("Cannot pass more than 1 arg");
            }

            if (!input.ValidateHistory(MailClientUtilities.GetHistory()))
            {
                Console.WriteLine("Found duplicate adresses. Continue?");

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.KeyChar.ToString().ToLower() != "y")
                {
                    return "Operation Cancelled";
                }
            }

            return MailClient.Run(input);
        }
    }
}