namespace MailClient.Toolkit.CLI
{
    internal abstract class Command<T,S> : CommandBase where T : Context,new() where S : Command<T,S>
    {
        protected string[] args;

        private T context;

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
        public override string Parse()
        {
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

                    if (result == string.Empty) result = arguments[i].Continue<T,S>(args,context);

                    return result;
                }
            }

            return $"{args[0]} is not a valid parameter";

        }

        public abstract List<Argument> GetArguments();
    }
}