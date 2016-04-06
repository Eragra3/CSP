using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class ExperimentResultStatistics
    {
        public readonly IList<RunResultStatistics> AllRuns;

        public readonly ConfigurationBatchFile Configuration;

        public ExperimentResultStatistics(ConfigurationBatchFile configuration, IList<RunResultStatistics> allRuns = null)
        {
            Configuration = configuration;
            AllRuns = allRuns ?? new List<RunResultStatistics>(configuration.MaxN + 1 - configuration.MinN);
        }
    }
}
