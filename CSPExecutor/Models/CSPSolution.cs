using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class CSPSolution
    {
        public readonly int[] Solution;

        public readonly int NumberOfBacktracks;

        public readonly int N;

        public CSPSolution(int[] solution, int numberOfBacktracks, int n)
        {
            Solution = solution;
            NumberOfBacktracks = numberOfBacktracks;
            N = n;
        }
    }
}
