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

        public ValuePickingHeuristicsEnum ValuePickingHeuristicMethod { get; }

        public VariablePickingHeuristicsEnum VariablePickingHeuristicMethod { get; }

        public ExecutorsEnum UsedExecutor { get; }

        public ConfigurationBatchFile(int minN, int maxN, ValuePickingHeuristicsEnum valuePickingMethod, VariablePickingHeuristicsEnum variablePickingMethod, ExecutorsEnum usedExecutor)
        {
            MinN = minN;
            MaxN = maxN;
            ValuePickingHeuristicMethod = valuePickingMethod;
            VariablePickingHeuristicMethod = variablePickingMethod;
            UsedExecutor = usedExecutor;
        }
    }
}
