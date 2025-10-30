namespace MailClient.code.Toolkit.CLI
{
    internal abstract class Param
    {
        protected string[] names;
        protected string description;
        protected Func<string[], string> function;

        private const string TO_STRING_FORMAT = "{0,-10} {1,-10} {2}";

        public Param(Func<string[], string> function, string description, params string[] names)
        {
            this.function = function;
            this.description = description;
            this.names = names;
        }

        /// <summary>
        /// Gets the longest name from the names array
        /// </summary>
        /// <returns>The longest name from the names array</returns>
        private string GetLongestName()
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

        public string GetName(int index)
        {
            return names[index];
        }

        /// <summary>
        /// Gets the params description
        /// </summary>
        /// <returns>The description</returns>
        private string GetDescription() => description;

        public override string ToString()
        {
            return string.Format(TO_STRING_FORMAT, $"{GetLongestName()}", "<value>", GetDescription());
        }

        // Compares the passed string to the names of this parameter. Returns true if at least one matches
        public bool TryCompare(string input, out int closestValue, out int closestIndex)
        {
            closestIndex = -1;
            closestValue = int.MinValue;

            for (int i = 0; i < names.Length; i++)
            {
                int compare = Math.Abs(names[i].CompareTo(input));

                if (compare == 0)
                {
                    closestIndex = i;
                    closestValue = compare;
                    return true;
                }

                if (compare <= closestValue)
                {
                    closestValue = compare;
                    closestIndex = i;
                }
            }

            return false;
        }

        /// <summary>
        /// Proccesses the passed input
        /// </summary>
        /// <param name="args">The remaining unproccessed arguments</param>
        /// <returns>A string value. Empty Means success and non-empty means failiure</returns>
        public virtual string Process(string[] args)
        {
            return function.Invoke(args);
        }

        /// <summary>
        /// Passes the remaining arguments to a new instance of a 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="args"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public abstract string Pass<T, S>(string[] args, Context cont) where T : Context, new() where S : Command<T, S>;

        protected string Pass<S, T>(string[] args, Context cont, int amountSkip) where T : Context, new() where S : Command<T, S>
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

        /// <summary>
        /// Returns a param with the closest name to what was entered
        /// </summary>
        /// <param name="input">The input to check all params against</param>
        /// <param name="params">The params to check the input against</param>
        /// <returns>The closest Param</returns>
        public static string GetClosestParam(string input, IEnumerable<Param> @params)
        {
            int closestScore = int.MaxValue;
            int closestIndex = -1;
            Param? closest = null;

            foreach (var item in @params)
            {
                item.TryCompare(input, out int _closestScore, out int _closestIndex);

                if (_closestScore <= closestScore)
                {
                    closestScore = _closestScore;
                    closestIndex = _closestIndex;
                    closest = item;
                }
            }

            return closest?.GetName(closestIndex) ?? throw new NullReferenceException("");
        }
    }
}