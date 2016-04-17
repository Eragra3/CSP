using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class SingleRunResultStatistics
    {
        public readonly IList<SolutionStatistics> AllRuns;

        public readonly IList<int[]> AllSolutions;

        public long RunsCount => AllRuns.Count;
        public long SolutionsCount => AllSolutions.Count;
        public long AllPossibleSolutionsCount => Configuration.AllSolutionsCount[N];
        public int MinNumberOfBacktracks => AllRuns.Any() ? AllRuns.Min(s => s.BacktracksCount) : 0;
        public int MaxNumberOfBacktracks => AllRuns.Any() ? AllRuns.Max(s => s.BacktracksCount) : 0;
        public double AverageNumberOfBacktracks => AllRuns.Any() ? AllRuns.Average(s => s.BacktracksCount) : 0;
        public int N { get; }
        public ExecutorsEnum UsedExecutor { get; }

        public SingleRunResultStatistics(int n, ExecutorsEnum usedExecutor, IList<SolutionStatistics> allRunsWithNewSolution = null)
        {
            N = n;
            UsedExecutor = usedExecutor;
            AllRuns = allRunsWithNewSolution ?? new List<SolutionStatistics>(1000);
            AllSolutions = new List<int[]>(100);
        }
    }
}
