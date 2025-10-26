namespace MailClient.Toolkit.CLI
{
    internal class Argument : Param
    {
        public Argument(Func<string[], string> function, string description, params string[] names) : base(function, description, names) { }

        public override string Continue<S, T>(string[] args, Context cont)
        {
            object[] arguments = new object[] { args.Skip(2).ToArray(), cont };

            Type t = typeof(T);

            object? o = Activator.CreateInstance(t, arguments);

            if (o == null) throw new NotImplementedException();

            if (o is T command)
            {
                return command.Parse();
            }

            return "Error: Activator.CreateInstance created an instance not of command type";
        }
    }
}
