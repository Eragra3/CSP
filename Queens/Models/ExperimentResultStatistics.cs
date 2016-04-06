using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class RunResultStatistics
    {
        public readonly IList<SolutionStatistics> AllRunsWithNewSolution;

        public int RunsWithNoNewSolutionCount;

        public int RunsCount => AllRunsWithNewSolution.Count + RunsWithNoNewSolutionCount;

        public readonly int N;

        public RunResultStatistics(int n, IList<SolutionStatistics> allRunsWithNewSolution = null)
        {
            N = n;
            AllRunsWithNewSolution = allRunsWithNewSolution ?? new List<SolutionStatistics>(1000);
        }
    }
}
