using System.Text;

namespace MailClient.Toolkit.CLI
{
    internal abstract class CommandBase
    {
        protected const string ARG_PROCESS_SUCCES = "";

        // The code for executing a command. Throws CommandExecutionException if failed
        public abstract string Execute();

        // Parses the incomming command parameter
        public abstract string Parse();

        // Returns all arguments that the command has
        public abstract (string name, string desc, List<Argument> args) GetData();

        public static IEnumerable<Type> GetAllCommands()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsSubclassOf(typeof(CommandBase)) && !x.IsAbstract);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine("Command: " + GetData().name);
            sb.AppendLine($"\tDescription: {GetData().desc}");
            sb.AppendLine($"\t\tParameters:");

            foreach (var item1 in GetData().args)
            {
                sb.AppendLine($"\t\t\t{item1.ToString()}, ");
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}
