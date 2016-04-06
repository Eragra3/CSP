using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Extensions
{
    public static class ExtensionMethods
    {
        public static string Reverse(this string text)
        {
            if (text == null) return null;

            // this was posted by petebob as well 
            var array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
    }
}
