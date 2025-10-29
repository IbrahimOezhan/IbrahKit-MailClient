namespace MailClient.Toolkit.CLI
{
    internal class Flag : Param
    {
        public Flag(Func<string[], string> function, string description, params string[] names) : base(function, description, names)
        {
        }

        public override string Continue<T, S>(string[] args, Context cont)
        {
            return Continue<S, T>(args, cont, 1);
        }

        public override string ProcessArg(string[] args)
        {
            return base.ProcessArg(args);
        }
    }
}
