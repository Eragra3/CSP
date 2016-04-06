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

        #region solutions count
        /// <summary>
        /// zero based table with maximum fundamental solutions
        /// e.g. for n = 8 there are 12 fundamental solutions
        /// </summary>
        public static long[] FundamentalSolutionsCount = {
            0, 1, 0, 0, 1, 2, 1, 6, 12, 46, 92, 341, 1787, 9233, 45752, 285053, 1846955,
            11977939, 83263591, 621012754, 4878666808, 39333324973, 336376244042,
            3029242658210, 28439272956934, 275986683743434, 2789712466510289
        };
        #endregion

        public static int BoardSize = 8;

        public static RowPickingHeuristicsEnum RowPickingHeuristic = RowPickingHeuristicsEnum.Increment;

        public static bool ShowStatisticsWindow = true;

        public static bool RenderBoard = true;
    }

    public enum RowPickingHeuristicsEnum
    {
        Increment,
        Random
    }

    public enum QueenPickingHeuristicsEnum
    {
        Random
    }
}
