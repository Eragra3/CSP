using Queens.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens.Models
{
    public class ConfigurationBatchFile
    {
        public int MinN { get; }

        public int MaxN { get; }

        public RowPickingHeuristicsEnum RowPickingHeuristicMethod { get; }

        public QueenPickingHeuristicsEnum QueenPickingHeuristicMethod { get; }

        public ConfigurationBatchFile(int minN, int maxN, RowPickingHeuristicsEnum rowPickingMethod, QueenPickingHeuristicsEnum queenPickingMethod)
        {
            MinN = minN;
            MaxN = maxN;
            RowPickingHeuristicMethod = rowPickingMethod;
            QueenPickingHeuristicMethod = queenPickingMethod;
        }
    }
}
