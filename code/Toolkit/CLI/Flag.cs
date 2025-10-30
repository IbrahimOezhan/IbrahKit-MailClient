namespace MailClient.code.Toolkit.CLI
{
    internal class Flag : Param
    {
        public Flag(Func<string[], string> function, string description, params string[] names) : base(function, description, names)
        {
        }

        public override string Pass<T, S>(string[] args, Context cont)
        {
            return Pass<S, T>(args, cont, 1);
        }

        public override string Process(string[] args)
        {
            return base.Process(args);
        }
    }
}
