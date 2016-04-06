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
        public static int BoardSize = 8;

        public static RowPickingHeuristicsEnum RowPickingHeuristic = RowPickingHeuristicsEnum.Increment;

        public static bool ShowStatisticsWindow = true;

        public static bool RenderBoard = true;
    }
}
