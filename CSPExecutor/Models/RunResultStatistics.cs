using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class ExperimentResultStatistics
    {
        public readonly IList<SingleRunResultStatistics> AllRuns;

        public readonly ConfigurationBatchFile Configuration;

        public ExperimentResultStatistics(ConfigurationBatchFile configuration, IList<SingleRunResultStatistics> allRuns = null)
        {
            Configuration = configuration;
            AllRuns = allRuns ?? new List<SingleRunResultStatistics>(configuration.MaxN + 1 - configuration.MinN);
        }
    }
}
