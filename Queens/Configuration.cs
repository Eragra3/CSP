using Queens.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens
{
    public static class Configuration
    {
        #region QUEENS

        /// <summary>
        /// zero based table with maximum fundamental solutions
        /// e.g. for n = 8 there are 12 fundamental solutions
        /// </summary>
        public static long[] FundamentalSolutionsCount =
        {
            0, 1, 0, 0, 1, 2, 1, 6, 12, 46, 92, 341, 1787, 9233, 45752, 285053, 1846955,
            11977939, 83263591, 621012754, 4878666808, 39333324973, 336376244042,
            3029242658210, 28439272956934, 275986683743434, 2789712466510289
        };

        public static long[] AllSolutionsCount =
        {
            0, 1, 0, 0, 2, 10, 4, 40, 92, 352, 724, 2680, 14200, 73712,
            365596, 2279184, 14772512, 95815104, 666090624,
            4968057848, 39029188884, 314666222712, 2691008701644,
            24233937684440, 227514171973736, 2207893435808352,
            22317699616364044
        };


        private static int[] _queensDomainCache;
        private static int? _boardSizeCache;

        public static int[] QueensDomain
        {
            get
            {
                if (_boardSizeCache.HasValue && _boardSizeCache == BoardSize)
                    return _queensDomainCache;

                _queensDomainCache = new int[BoardSize];
                var number = 0;
                for (int i = 0; i < BoardSize; i++)
                {
                    _queensDomainCache[i] = number++;
                }

                return _queensDomainCache;
            }
        }

        #endregion


        public static int BoardSize = 8;

        public static ValuePickingHeuristicsEnum ValuePickingHeuristic = ValuePickingHeuristicsEnum.Random;

        public static VariablePickingHeuristicsEnum VariablePickingHeuristic = VariablePickingHeuristicsEnum.Increment;

        public static bool ShowStatisticsWindow = true;

        public static bool RenderBoard = true;

        public static int MaxExperimentTimeInSeconds = 120;
    }

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
