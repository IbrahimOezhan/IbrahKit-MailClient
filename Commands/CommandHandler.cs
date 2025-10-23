namespace MailClient.Commands
{
    internal class CommandHandler
    {
        public string Run(string[] args)
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

            return "";
        }
    }
}
