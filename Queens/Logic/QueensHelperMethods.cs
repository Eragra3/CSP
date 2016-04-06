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

        #region solutions count

        public static long[] FundamentalSolutionsCount = {
            1, 0, 0, 1, 2, 1, 6, 12, 46, 92, 341, 1787, 9233, 45752, 285053, 1846955,
            11977939, 83263591, 621012754, 4878666808, 39333324973, 336376244042,
            3029242658210, 28439272956934, 275986683743434, 2789712466510289
        };
        #endregion

        public static bool Conflicts(int i1, int j1, int i2, int j2)
        {
            return i1 == i2 || j1 == j2 || Math.Abs(i1 - i2) == Math.Abs(j1 - j2);
        }

        public static int GetRandomRow(List<int> possibleRows)
        {
            return possibleRows.ElementAt(_random.Next(0, possibleRows.Count));
        }

        public static void AssertSolutionCorrect(int[] solutionRows)
        {
            if (solutionRows == null) return;
            for (var i1 = 0; i1 < solutionRows.Length; i1++)
            {
                var j1 = solutionRows[i1];
                for (var i2 = 0; i2 < solutionRows.Length; i2++)
                {
                    if (i2 == i1) continue;
                    var j2 = solutionRows[i2];
                    if (QueensHelperMethods.Conflicts(i1, j1, i2, j2))
                    {
                        throw new Exception("Solution is invalid!");
                    }
                }
            }
        }
    }
    public enum RowPickingHeuristicsEnum
    {
        Increment,
        Random
    }
}
