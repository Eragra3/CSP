using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Queens.Logic
{
    public class QueensHelperMethods
    {
        private static Random _random = new Random();

        public static bool Conflicts(int i1, int j1, int i2, int j2)
        {
            return i1 == i2 || j1 == j2 || Math.Abs(i1 - i2) == Math.Abs(j1 - j2);
        }

        public static int GetRandomRow(List<int> possibleRows)
        {
            return possibleRows.ElementAt(_random.Next(0, possibleRows.Count));
        }
    }
    public enum RowPickingHeuristicsEnum
    {
        Increment,
        Random
    }
}
