using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient.Toolkit.CLI
{
    internal class Argument : Param
    {
        public Argument(Func<string[], string> function, string description, params string[] names) : base(function, description, names) { }

        public override string Continue<S, T>(string[] args, Context cont)
        {
            object[] arguments = new object[] { args.Skip(2).ToArray(), cont };

            T? g = (T)Activator.CreateInstance(typeof(T),arguments);

            return g.Parse();
        }
    }
}
