using MailClient.code.Toolkit.CLI.Exceptions;

namespace MailClient.code.Toolkit.CLI
{
    internal class Argument : Param
    {
        public Argument(Func<string[], string> function, string description, params string[] names) : base(function, description, names)
        {

        }

        public override string Pass<T, S>(string[] args, Context cont)
        {
            return Pass<S, T>(args, cont, 2);
        }

        public override string Process(string[] args)
        {
            if (args.Length == 1)
            {
                throw new ArgumentParsingException($"No value for {args[0]} parameter provided");
            }

            return base.Process(args);
        }
    }
}
