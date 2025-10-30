namespace MailClient.Toolkit.CLI
{
    internal abstract class Command<T, S> : CommandBase where T : Context, new() where S : Command<T, S>
    {
        protected string[] parameters;

        private T context;

        private const string INVALID_PARAM = "{0} is an invalid paramter. Did you mean {1}?";

        public Command(string[] args)
        {
            this.parameters = args;
            context = new T();
        }

        public Command(string[] args, T context)
        {
            this.context = context;
            this.parameters = args;
        }

        public T GetContext() => context;

        /// <summary>
        /// Parses the incomming parameters
        /// </summary>
        /// <returns>An empty string if successfull</returns>
        public override string Parse()
        {
            //If input ends here returns string.empty
            if (parameters.Length == 0)
            {
                return ARG_PROCESS_SUCCES;
            }

            string next = parameters[0];

            List<Argument> arguments = GetData().args;

            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i].TryCompare(next, out _,out _))
                {
                    string result = arguments[i].Process(parameters);

                    result = result == ARG_PROCESS_SUCCES ? arguments[i].Pass<T, S>(parameters, context) : result;

                    return result;
                }
            }

            return string.Format(INVALID_PARAM, parameters[0], Param.GetClosestParam(next, arguments.Cast<Param>()));
        }
    }
}