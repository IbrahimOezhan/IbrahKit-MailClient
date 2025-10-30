namespace MailClient.Toolkit.CLI
{
    internal class CLI
    {
        private const string INVALID_COMMAND = "{0} is an invalid command. Use the help command to get a list of valid commands";

        public const string FOLDER = "IbrahKit";

        public const char SPLIT_AT_SPACE = ' ';

        private const string BEFORE_INPUT = "> ";

        private const string COMMAND_NULL_ERROR = "Fatal Error: Command is null";

        public void Run(string[] args)
        {
            string result = string.Empty;

            do
            {
                while (args.Length == 0)
                {
                    Console.Write(BEFORE_INPUT);

                    string? input = Console.ReadLine();

                    Console.WriteLine();

                    args = input != null ? input.Split(SPLIT_AT_SPACE) : Array.Empty<string>();
                }

                if (!TryGetCommand(args, out CommandBase? command))
                {
                    result = string.Format(INVALID_COMMAND, args[0]);
                }
                else
                {
                    result = command.Parse();
                    // Empty result means success.
                    // Non empty means error during argument value proccessing so skip execution and print the error
                    if (result == string.Empty) result = command.Execute();
                }

                Console.WriteLine(result);

                args = Array.Empty<string>();
            }
            while (result != null);
        }

        private static bool TryGetCommand(string[] args, out CommandBase result)
        {
            IEnumerable<Type> commandTypes = CommandBase.GetAllCommands();

            object[] arguments = new object[] { args.Skip(1).ToArray() };

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, arguments) is CommandBase command && args[0].ToLower().Equals(command.GetData().name))
                {
                    result = command;
                    return true;
                }
            }

            result = new HelpCommand(new string[0]);

            return false;
        }
    }
}
