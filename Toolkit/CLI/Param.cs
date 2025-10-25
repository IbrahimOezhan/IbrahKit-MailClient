using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MailClient.Toolkit.CLI
{
    internal abstract class Param
    {
        protected string[] names;
        protected string description;
        protected Func<string[], string> function;

        public Param(Func<string[],string> function, string description,params string[] names)
        {
            this.function = function;
            this.description = description;
            this.names = names;
        }

        public bool CompareArg(string input)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Equals(input)) return true;
            }

            return false;
        }

        public string ProcessArg(string[] args)
        {
            return function.Invoke(args);
        }

        public abstract string Continue<T, S>(string[] args, Context cont) where T : Context, new() where S : Command<T, S>;
    }
}
