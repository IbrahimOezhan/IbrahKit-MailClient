using MailClient.Toolkit.CLI.Exceptions;

namespace MailClient.Toolkit.CLI
{
    internal class Argument : Param
    {
        public Argument(Func<string[], string> function, string description, params string[] names) : base(function, description, names)
        {

        }

        public override string Continue<T, S>(string[] args, Context cont)
        {
            return Continue<S, T>(args, cont, 2);
        }

        public override string ProcessArg(string[] args)
        {
            if (args.Length == 1)
            {
                throw new ArgumentParsingException($"No value for {args[0]} parameter provided");
            }

            return base.ProcessArg(args);
        }
    }
}
