namespace Sudoku.Models
{
    public class CSPSolution
    {
        public readonly int[] Solution;

        public readonly int BacktracksCount;

        public readonly int HolesCount;

        public readonly int BoardSize;

        public CSPSolution(int[] solution, int backtracksCount, int holesCount, int boardSize)
        {
            Solution = solution;
            BacktracksCount = backtracksCount;
            HolesCount = holesCount;
            BoardSize = boardSize;
        }
    }
}
