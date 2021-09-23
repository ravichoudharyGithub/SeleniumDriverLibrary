using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDriverLibrary.Internal
{
    internal static class Helper
    {
        public static string ExtractAfterEquals(string field)
        {
            var posequals = field.IndexOf('=');
            if (posequals >= 0)
                return field.Substring(posequals + 1);
            // nothing found, just return main part
            return field;
        }

        public static string GetIdentifierBeforeEquals(string field)
        {
            var index = field.IndexOf("=", StringComparison.CurrentCulture);
            return field.Remove(index);
        }
    }
}
