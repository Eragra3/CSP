using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class SolutionStatistics
    {
        public readonly int[] Solution;

        public readonly int NumberOfBacktracks;

        public readonly int N;

        public readonly int NumberOfNewSolutions;

        public SolutionStatistics(int[] solution, int numberOfBacktracks, int n, int numberOfNewSolutions)
        {
            Solution = solution;
            NumberOfBacktracks = numberOfBacktracks;
            N = n;
            NumberOfNewSolutions = numberOfNewSolutions;
        }
    }
}
