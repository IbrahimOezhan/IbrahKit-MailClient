namespace MailClient.Toolkit.CLI
{
    internal abstract class CommandBase
    {
        // Defines the name of the command used for calling it
        public abstract string GetCommand();

        // The code for executing a command. Throws CommandExecutionException if failed
        public abstract string Execute();

        // Parses the incomming command parameter
        public abstract string Parse();

        // Returns all arguments that the command has
        public abstract List<Argument> GetArguments();
    }
}
