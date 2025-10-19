using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    public static class StringUtilities
    {
        public static bool IsNullEmptyWhite(string text) => string.IsNullOrEmpty(text.Trim()) || string.IsNullOrWhiteSpace(text.Trim());
    }
}
