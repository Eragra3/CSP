using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Models
{
    public class SingleRunResultStatistics
    {
        public readonly IList<SolutionStatistics> AllRuns;

        public long RunsCount => AllRuns.Count;
        //public long AllPossibleSolutionsCount => Configuration.AllSolutionsCount[HolesCount];
        public int MinNumberOfBacktracks => AllRuns.Any() ? AllRuns.Min(s => s.BacktracksCount) : 0;
        public int MaxNumberOfBacktracks => AllRuns.Any() ? AllRuns.Max(s => s.BacktracksCount) : 0;
        public double AverageNumberOfBacktracks => AllRuns.Any() ? AllRuns.Average(s => s.BacktracksCount) : 0;
        public int BoardSize { get; }
        public Configuration.ExecutorsEnum UsedExecutor { get; }
        public int HolesNumber { get; }

        public SingleRunResultStatistics(int BoardSize, Configuration.ExecutorsEnum usedExecutor, int holesNumber, IList<SolutionStatistics> allRunsWithNewSolution = null)
        {
            this.BoardSize = BoardSize;
            UsedExecutor = usedExecutor;
            AllRuns = allRunsWithNewSolution ?? new List<SolutionStatistics>(1000);
            HolesNumber = holesNumber;
        }
    }
}
