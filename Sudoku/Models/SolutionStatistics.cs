namespace Sudoku.Models
{
    public class SolutionStatistics
    {
        public readonly int BacktracksCount;

        public readonly int HolesCount;

        public readonly int BoardSize;

        public SolutionStatistics(int backtracksCount, int holesCount, int boardSize)
        {
            BacktracksCount = backtracksCount;
            HolesCount = holesCount;
            BoardSize = boardSize;
        }
    }
}
