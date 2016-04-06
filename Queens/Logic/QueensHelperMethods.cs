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

        /// <summary>
        /// return next possible int or -1
        /// </summary>
        /// <param name="possibleRows"></param>
        /// <returns></returns>
        public static int GetNextPossibleRow(List<int> possibleRows)
        {
            if (possibleRows.Count == 0) return -1;
            return possibleRows.Min();
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
                    if (Conflicts(i1, j1, i2, j2))
                    {
                        throw new Exception("Solution is invalid!");
                    }
                }
            }
        }
    }
}
