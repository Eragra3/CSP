using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Logic
{
    public class BacktrackingExecutor
    {
        public static int[] FindSolution(int n)
        {
            var solution = new int[n];

            var forbiddenRows = new int[n][];
            for (var i = 0; i < forbiddenRows.Length; i++)
            {
                forbiddenRows[i] = new int[n];
            }

            var lel = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = lel++;
            }

            return solution;
        }
    }
}
