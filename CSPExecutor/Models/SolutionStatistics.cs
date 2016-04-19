using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class SolutionStatistics
    {
        //public readonly int[] Solution;

        public readonly int BacktracksCount;

        public readonly int N;

        public SolutionStatistics(int[] solution, int backtracksCount, int n)
        {
            //Solution = solution;
            BacktracksCount = backtracksCount;
            N = n;
        }
    }
}
