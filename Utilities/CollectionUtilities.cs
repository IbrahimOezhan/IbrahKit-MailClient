using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbrahKit_MailClient.Utilities
{
    internal static class CollectionUtilities
    {
        public static string OutputEnum<T>(Type en)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                stringBuilder.Append(item.ToString() + ", ");
            }

            return stringBuilder.ToString();
        }
    }
}
