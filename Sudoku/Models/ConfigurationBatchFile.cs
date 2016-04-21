namespace Sudoku.Models
{
    public class ConfigurationBatchFile
    {
        public int MinHoles { get; }

        public int MaxHoles { get; }

        public int BoardSize { get; }

        public Configuration.ValuePickingHeuristicsEnum ValuePickingHeuristicMethod { get; }

        public Configuration.VariablePickingHeuristicsEnum VariablePickingHeuristicMethod { get; }

        public Configuration.ExecutorsEnum UsedExecutor { get; }

        public ConfigurationBatchFile(int minHoles, int maxHoles, Configuration.ValuePickingHeuristicsEnum valuePickingMethod, Configuration.VariablePickingHeuristicsEnum variablePickingMethod, Configuration.ExecutorsEnum usedExecutor, int boardSize)
        {
            MinHoles = minHoles;
            MaxHoles = maxHoles;
            ValuePickingHeuristicMethod = valuePickingMethod;
            VariablePickingHeuristicMethod = variablePickingMethod;
            UsedExecutor = usedExecutor;
            BoardSize = boardSize;
        }
    }
}
