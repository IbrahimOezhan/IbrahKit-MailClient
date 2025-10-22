using System.Diagnostics;
using System.Text.Json;

namespace MailClient.Utilities
{
    internal static class MainUtilities
    {
        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            WriteIndented = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
        };

        public static string GetModuleDir()
        {
            ProcessModule? module = Process.GetCurrentProcess().MainModule ?? throw new NullReferenceException("Error: Module is null");

            string? moduleProccessDir = Path.GetDirectoryName(module.FileName) ?? throw new Exception("Error: Module proccess dir is null");

            return moduleProccessDir;
        }

        public static string ForceInput(string initalMsg = "", string errorMsg = "")
        {
            Console.Write(initalMsg + " ");

            string? input = Console.ReadLine();

            while (input == null || input == string.Empty || input.Length == 0)
            {
                Console.WriteLine(errorMsg);

                Console.Write(initalMsg);

                input = Console.ReadLine();
            }

            return input;
        }

        public static bool InputYesNo(char yes, char no, string initalMsg = "", string errorMsg = "")
        {
            Console.WriteLine($"{initalMsg} Yes({yes}), No({no})");

            ConsoleKeyInfo input = Console.ReadKey();

            char inputChar = input.KeyChar;

            while (!(inputChar.ToString().Equals(yes.ToString(), StringComparison.OrdinalIgnoreCase) || inputChar.ToString().Equals(no.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine(errorMsg);

                input = Console.ReadKey();

                inputChar = input.KeyChar;
            }

            return inputChar.ToString().Equals(yes.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public static string FormattedException(Exception exception)
        {
            return "\nException: \n\n" + exception.ToString() + "\n\n";
        }

        public static JsonSerializerOptions GetJsonOptions() => options;

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(text);

            Console.ResetColor();
        }
    }
}