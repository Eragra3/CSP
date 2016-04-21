using System.Collections.Generic;

namespace Sudoku.Models
{
    public class ExperimentResultStatistics
    {
        public readonly IList<SingleRunResultStatistics> AllRuns;

        public readonly ConfigurationBatchFile Configuration;

        public ExperimentResultStatistics(ConfigurationBatchFile configuration, IList<SingleRunResultStatistics> allRuns = null)
        {
            Configuration = configuration;
            AllRuns = allRuns ?? new List<SingleRunResultStatistics>(configuration.MaxHoles + 1 - configuration.MinHoles);
        }
    }
}
