using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queens.Models;

namespace Queens.Logic
{
    public class BacktrackingExecutor
    {
        public static CSPSolution FindSolution(int n, RowPickingHeuristicsEnum rowPickingHeuristic)
        {
            var numberOfBacktracks = 0;

            var solutionRows = new int[n];
            for (var i = 0; i < solutionRows.Length; i++)
            {
                solutionRows[i] = int.MinValue;
            }

            var usedRowsLists = new IList<int>[n];
            for (var i = 0; i < usedRowsLists.Length; i++)
            {
                usedRowsLists[i] = new List<int>(n);
            }

            var allRows = new List<int>();
            for (var i = 0; i < n; i++)
            {
                allRows.Add(i);
            }
            var usedRows = new List<int>();

            for (var columnIndex = 0; columnIndex < solutionRows.Length; columnIndex++)
            {
                var rowIndex = -1;
                usedRows.Clear();

                var noRowFound = false;

                while (solutionRows[columnIndex] == int.MinValue)
                {
                    switch (rowPickingHeuristic)
                    {
                        case RowPickingHeuristicsEnum.Increment:
                            rowIndex++;
                            noRowFound = rowIndex == n;
                            break;
                        case RowPickingHeuristicsEnum.Random:
                            if (allRows.Count == usedRows.Count)
                                noRowFound = true;
                            else
                                rowIndex = QueensHelperMethods.GetRandomRow(
                                    allRows.Except(usedRows).ToList());
                            break;
                        default:
                            break;
                    }
                    if (noRowFound)
                    {
                        solutionRows[columnIndex] = int.MinValue;
                        break;
                    }

                    if (usedRowsLists[columnIndex].Contains(rowIndex))
                    {
                        usedRows.Add(rowIndex);
                        continue;
                    }

                    var conflicts = false;
                    for (var k = 0; !conflicts && k < columnIndex; k++)
                    {
                        conflicts = QueensHelperMethods.Conflicts(k, solutionRows[k], columnIndex, rowIndex);
                    }

                    if (conflicts)
                    {
                        usedRows.Add(rowIndex);
                    }
                    else
                    {
                        solutionRows[columnIndex] = rowIndex;
                    }

                }

                //go back
                if (solutionRows[columnIndex] == int.MinValue)
                {
                    numberOfBacktracks++;
                    usedRowsLists[columnIndex].Clear();

                    columnIndex--;

                    if (columnIndex < 0)
                    {
                        return new CSPSolution()
                        {
                            NumberOfBacktracks = numberOfBacktracks
                        };
                    }

                    usedRowsLists[columnIndex].Add(solutionRows[columnIndex]);
                    solutionRows[columnIndex] = int.MinValue;

                    columnIndex--;
                }

            }

            var solution = new CSPSolution()
            {
                Solution = solutionRows,
                NumberOfBacktracks = numberOfBacktracks
            };
            return solution;
        }
    }
}
