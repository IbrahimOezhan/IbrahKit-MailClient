using MailClient.Commands;
using System.Text;

namespace MailClient.Main
{
    internal class MailClient
    {
        public const string FOLDER = "IbrahKit";

        public void Run(string[] args)
        {
            string res = "";

            do
            {
                if (args.Length == 0)
                {
                    string? input = Console.ReadLine();

                    args = input.Split(' ');
                }

                Command command = GetCommand(args);

                res = command.Parse();

                if(res == string.Empty) res = command.Execute();

                Console.WriteLine(res);

                args = Array.Empty<string>();
            }
            while (res != null);
        }

        public Command GetCommand(string[] args)
        {
            IEnumerable<Type> commandTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsSubclassOf(typeof(Command)));

            object[] arguments = new object[] { args.Skip(1).ToArray() };

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, arguments) is Command command)
                {
                    if (args[0].ToLower().Equals(command.GetCommand()))
                    {
                        return command;
                    }
                }
            }

            StringBuilder sb = new(args[0] + "is an invalid command. Valid commands are:");

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, arguments) is Command command)
                {
                    sb.AppendLine(command.GetCommand());
                }
            }

            return sb.ToString();
        }
    }
}
