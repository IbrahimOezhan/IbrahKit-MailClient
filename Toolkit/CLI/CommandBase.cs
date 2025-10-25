using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient.Toolkit.CLI
{
    internal abstract class CommandBase
    {
        public abstract string GetCommand();

        public abstract string Execute();

        public abstract string Parse();
    }
}
