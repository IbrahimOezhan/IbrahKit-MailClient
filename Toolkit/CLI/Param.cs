namespace MailClient.Toolkit.CLI
{
    internal abstract class Param
    {
        protected string[] names;
        protected string description;
        protected Func<string[], string> function;

        public Param(Func<string[], string> function, string description, params string[] names)
        {
            this.function = function;
            this.description = description;
            this.names = names;
        }

        public override string ToString()
        {
            return string.Format("{0,-10} {1,-10} {2}",$"{GetLongestName()}","<value>",GetDescription());
        }

        public string GetLongestName()
        {
            int length = int.MinValue;

            int index = -1;

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Length > length)
                {
                    length = names[i].Length;
                    index = i;
                }
            }

            return names[index];
        }

        public string GetDescription()
        {
            return description;
        }

        // Finds the most smiliar name compared to the one passed as an argument and returns it as well as the CompareTo int value
        public (int, string) CompareTo(string name)
        {
            int smallest = int.MaxValue;
            string smallestValue = string.Empty;

            for (int i = 0; i < names.Length; i++)
            {
                int compare = Math.Abs(names[i].CompareTo(name));

                if (compare < smallest)
                {
                    smallest = compare;

                    smallestValue = names[i];
                }
            }

            return (smallest, smallestValue);
        }

        // Compares the passed string to the names of this parameter. Returns true if at least one matches
        public bool CompareArg(string input)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Equals(input)) return true;
            }

            return false;
        }

        //Process the argument.
        // An empty return value means success
        //This usually involves setting the provided value to the context object.
        //If that fails because no value is provided a non empty value is returned.
        public virtual string ProcessArg(string[] args)
        {
            return function.Invoke(args);
        }

        protected string Continue<S, T>(string[] args, Context cont, int amountSkip) where T : Context, new() where S : Command<T, S>
        {
            object[] arguments = [args.Skip(amountSkip).ToArray(), cont];

            Type t = typeof(S);

            object? o = Activator.CreateInstance(t, arguments) ?? throw new NullReferenceException();

            if (o is S command)
            {
                return command.Parse();
            }

            return "Error: Activator.CreateInstance created an instance not of command type";
        }

        public abstract string Continue<T, S>(string[] args, Context cont) where T : Context, new() where S : Command<T, S>;

        public static string GetClosestArg(string arg, IEnumerable<Param> arguments)
        {
            int closest = int.MaxValue;

            string closestValue = string.Empty;

            foreach (var item in arguments)
            {
                (int compare, string value) = item.CompareTo(arg);

                compare = Math.Abs(compare);

                if (compare <= closest)
                {
                    closest = compare;
                    closestValue = value;
                }
            }

            return closestValue;
        }
    }
}
