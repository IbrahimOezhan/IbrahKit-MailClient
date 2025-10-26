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

        //Process the argument. This usually involves setting the provided value to the context object.
        //If that fails because no value is provided a non empty value is returned.
        public string ProcessArg(string[] args)
        {
            return function.Invoke(args);
        }

        public abstract string Continue<T, S>(string[] args, Context cont) where T : Context, new() where S : Command<T, S>;
    }
}
