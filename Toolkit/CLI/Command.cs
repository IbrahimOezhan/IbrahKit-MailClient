namespace MailClient.Toolkit.CLI
{
    internal abstract class Command<T, S> : CommandBase where T : Context, new() where S : Command<T, S>
    {
        protected string[] args;

        private T context;

        private const string INVALID_PARAM = "{0} is an invalid paramter";

        public Command(string[] args)
        {
            this.args = args;
            context = new T();
        }

        public Command(string[] args, T context)
        {
            this.context = context;
            this.args = args;
        }

        public T GetContext()
        {
            return context;
        }

        //Parses the next arguments
        public override string Parse()
        {
            //If input ends here returns string.Empty to signalise the CLI to process the command
            if (args.Length == 0)
            {
                return string.Empty;
            }

            List<Argument> arguments = GetArguments();

            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i].CompareArg(args[0]))
                {
                    string result = arguments[i].ProcessArg(args);

                    // If the argument processing returned string.empty that means it was successfull
                    // In that case call Continue on the argument object which creates a new command and passes the arguments on
                    if (result == string.Empty) result = arguments[i].Continue<T, S>(args, context);

                    // If this line is reached that means the returned value was not empty therefor there was an error and its returned to the CLI
                    return result;
                }
            }

            //If the code reached this it means the argument is not known to the command. Return error message
            return string.Format(INVALID_PARAM,args[0]);
        }

        public abstract List<Argument> GetArguments();
    }
}