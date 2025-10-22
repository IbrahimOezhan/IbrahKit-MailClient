using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal abstract class CommandState
    {
        protected string[] args;

        public CommandState(string[] args)
        {
            this.args = args;
        }

        public abstract string Run();
    }
}
