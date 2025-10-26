namespace MailClient.Toolkit.CLI
{
    internal class CLI
    {
        private const string INVALID_COMMAND = "{0} is an invalid command. Use the help command to get a list of valid commands";

        public const string FOLDER = "IbrahKit";

        public void Run(string[] args)
        {
            string result = string.Empty;

            bool first = false;

            do
            {
                args = first ? Array.Empty<string>() : args;

                first = true;

                while (args.Length == 0)
                {
                    string? input = Console.ReadLine();

                    args = input != null ? input.Split(' ') : Array.Empty<string>();
                }

                if (!TryGetCommand(args, out CommandBase? command))
                {
                    Console.WriteLine(INVALID_COMMAND, args[0]);
                    continue;
                }

                if (command == null)
                {
                    result = "Fatal error";
                    continue;
                }

                result = command.Parse();

                // Empty result means success.
                // Non empty means error during argument value proccessing so skip execution and print the error
                if (result == string.Empty) result = command.Execute();

                Console.WriteLine(result);
            }
            while (result != null);
        }

        private static bool TryGetCommand(string[] args, out CommandBase? result)
        {
            IEnumerable<Type> commandTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
            x => x.GetTypes()).Where(x => x.IsSubclassOf(typeof(CommandBase)) && !x.IsAbstract);

            object[] arguments = new object[] { args.Skip(1).ToArray() };

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, arguments) is CommandBase command && args[0].ToLower().Equals(command.GetCommand()))
                {
                    result = command;
                    return true;
                }
            }

            result = null;

            return false;
        }
    }
}
