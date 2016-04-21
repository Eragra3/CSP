using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class ExtensionMethods
    {
        public static T[] Shuffle<T>(this Random rng, T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                var k = rng.Next(n--);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }

        public static string Reverse(this string text)
        {
            if (text == null) return null;

            // this was posted by petebob as well 
            var array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}
