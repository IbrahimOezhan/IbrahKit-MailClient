using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal class ProfileContext
    {
        private string name;

        private Mode mode;

        internal enum Mode
        {
            LIST,
            CREATE,
            DELETE,
        }

        public string GetName()
        {
            return name;
        }

        public Mode GetMode()
        {
            return mode;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetMode(Mode mode)
        {
            this.mode = mode;
        }
    }
}
