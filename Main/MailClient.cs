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
                res = Command(args);

                Console.WriteLine(res);
            }
            while (res != null);
        }

        public string Command(string[] args)
        {
            IEnumerable<Type> commandTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsAssignableTo(typeof(Command)));

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, [.. args.Skip(1)]) is Command command)
                {
                    if (args[0].ToLower().Equals(command.CommandName()))
                    {
                        return command.Run();
                    }
                }
            }

            StringBuilder sb = new(args[0] + "is an invalid command. Valid commands are:");

            foreach (var item in commandTypes)
            {
                if (Activator.CreateInstance(item, [.. args.Skip(1)]) is Command command)
                {
                    sb.AppendLine(command.CommandName());
                }
            }

            return sb.ToString();
        }
    }
}
