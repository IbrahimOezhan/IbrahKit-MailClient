using System.Text;

namespace MailClient.Toolkit.CLI
{
    internal class CLI
    {
        private const string INVALID_COMMAND = "{0} is an invalid command. Use the help command to get a list of valid commands";

        public const string FOLDER = "IbrahKit";

        public void Run(string[] args)
        {
            string res = string.Empty;

            do
            {
                if (args.Length == 0)
                {
                    string? input = Console.ReadLine();

                    args = input.Split(' ');
                }

                if(!TryGetCommand(args,out CommandBase result))
                {
                    Console.WriteLine(INVALID_COMMAND, args[0]);
                    continue;
                }

                res = result.Parse();

                if(res == string.Empty) res = result.Execute();

                Console.WriteLine(res);

                args = Array.Empty<string>();
            }
            while (res != null);
        }

        public bool TryGetCommand(string[] args, out CommandBase result)
        {
            IEnumerable<Type> commandTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
            x => x.GetTypes()).Where(x => x.IsSubclassOf(typeof(CommandBase)) && !x.IsAbstract);

            object[] arguments = new object[] { args.Skip(1).ToArray() };

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, arguments) is CommandBase command)
                {
                    if (args[0].ToLower().Equals(command.GetCommand()))
                    {
                        result = command;
                        return true;
                    }
                }
            }

            result = null;

            return false;
        }
    }
}
