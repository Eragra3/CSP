using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Configuration
    {
        private static readonly Random _random = new Random();

        public static int GuiBoardSize;

        public const int BOARD_SIZE = 9;
        public const int HOLES_COUNT = 16;
        public static readonly int SUBBOARD_SIZE = (int) Math.Sqrt(BOARD_SIZE);

        private static int[] _sudokuDomainCache;
        private static int? _boardSizeCache;

        public static int[] SudokuDomain
        {
            get
            {
                if (_boardSizeCache.HasValue && _boardSizeCache == GuiBoardSize)
                    return _sudokuDomainCache;

                _sudokuDomainCache = new int[GuiBoardSize];
                var number = 1;
                for (var i = 0; i < GuiBoardSize; i++)
                {
                    _sudokuDomainCache[i] = number++;
                }

                return _sudokuDomainCache;
            }
        }


        #region EXAMPLE SUDOKU SOLUTIONS_3x3

        #region 3x3

        public static readonly int[][] SOLUTION_3x3_1 =
            {
                new [] {4,3,5,2,6,9,7,8,1},
                new [] {6,8,2,5,7,1,4,9,3},
                new [] {1,9,7,8,3,4,5,6,2},
                new [] {8,2,6,1,9,5,3,4,7},
                new [] {3,7,4,6,8,2,9,1,5},
                new [] {9,5,1,7,4,3,6,2,8},
                new [] {5,1,9,3,2,6,8,7,4},
                new [] {2,4,8,9,5,7,1,3,6},
                new [] {7,6,3,4,1,8,2,5,9}
            };

        public static int[][] GetRandom3x3Sudoku => SOLUTIONS_3x3[_random.Next(0, SOLUTIONS_3x3.Length)];
        public static readonly int[][][] SOLUTIONS_3x3 =
        {
            SOLUTION_3x3_1
        };

        #endregion

        #endregion


        public enum ValuePickingHeuristicsEnum
        {
            Increment = 0,
            Random = 1
        }

        public enum VariablePickingHeuristicsEnum
        {
            Increment = 0,
            Random = 1
        }

        public enum ExecutorsEnum
        {
            Backtracking = 0,
            ForwardChecking = 1
        }
    }
}
