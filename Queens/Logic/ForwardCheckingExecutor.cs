using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queens.Models;

namespace Queens.Logic
{
    public class ForwardCheckingExecutor
    {

        public static CSPSolution FindSolution(int n, RowPickingHeuristicsEnum rowPickingHeuristic)
        {
            var numberOfBacktracks = 0;

            var solutionRows = new int[n];
            for (var i = 0; i < solutionRows.Length; i++)
            {
                solutionRows[i] = int.MinValue;
            }

            var forbiddenLists = new HashSet<int>[n];
            var usedRowsLists = new HashSet<int>[n];

            for (var i = 0; i < forbiddenLists.Length; i++)
            {
                forbiddenLists[i] = new HashSet<int>();
                usedRowsLists[i] = new HashSet<int>();
            }

            var allRows = new List<int>();
            for (var i = 0; i < n; i++)
            {
                allRows.Add(i);
            }
            var possibleRows = new List<int>();

            for (var columnIndex = 0; columnIndex < solutionRows.Length; columnIndex++)
            {
                var rowIndex = 0;
                possibleRows = allRows
                    .Except(forbiddenLists[columnIndex])
                    .Except(usedRowsLists[columnIndex])
                    .ToList();


                var noRowFound = false;

                switch (rowPickingHeuristic)
                {
                    case RowPickingHeuristicsEnum.Increment:
                        while ((forbiddenLists[columnIndex].Contains(rowIndex) ||
                            usedRowsLists[columnIndex].Contains(rowIndex)) &&
                            !noRowFound)
                        {
                            rowIndex++;
                            noRowFound = rowIndex == n;
                            continue;
                        }
                        break;
                    case RowPickingHeuristicsEnum.Random:
                        if (possibleRows.Count == 0)
                            noRowFound = true;
                        else
                            rowIndex = QueensHelperMethods.GetRandomRow(possibleRows);
                        break;
                    default:
                        break;
                }

                solutionRows[columnIndex] = rowIndex;

                if (noRowFound)
                {
                    solutionRows[columnIndex] = int.MinValue;
                }

                if (solutionRows[columnIndex] != int.MinValue)
                    UpdateForbiddenLists(n, solutionRows, forbiddenLists, columnIndex + 1);

                //go back
                if (solutionRows[columnIndex] == int.MinValue || forbiddenLists.Any(l => l.Count == n))
                {
                    do
                    {
                        numberOfBacktracks++;

                        usedRowsLists[columnIndex].Clear();

                        foreach (var forbiddenList in forbiddenLists)
                        {
                            forbiddenList.Clear();
                        }

                        columnIndex--;
                        UpdateForbiddenLists(n, solutionRows, forbiddenLists, columnIndex);

                        if (columnIndex < 0)
                        {
                            return new CSPSolution()
                            {
                                NumberOfBacktracks = numberOfBacktracks
                            };
                        }

                        usedRowsLists[columnIndex].Add(solutionRows[columnIndex]);
                        solutionRows[columnIndex] = int.MinValue;

                    } while (forbiddenLists.Any(l => l.Count == n));

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

        private static void UpdateForbiddenLists(int n, int[] solutionRows, HashSet<int>[] forbiddenLists, int nextIndex)
        {
            for (var forbiddenColorIndex = 0; forbiddenColorIndex < nextIndex; forbiddenColorIndex++)
            {
                var forbiddenColor = solutionRows[forbiddenColorIndex];

                var diagonalUp = forbiddenColor + 1 + (nextIndex - 1 - forbiddenColorIndex);
                var diagonalDown = forbiddenColor - 1 - (nextIndex - 1 - forbiddenColorIndex);

                for (var j = nextIndex; j < solutionRows.Length; j++)
                {
                    forbiddenLists[j].Add(forbiddenColor);
                    var v1 = diagonalUp;
                    if (v1 < n)
                    {
                        forbiddenLists[j].Add(v1);
                        diagonalUp++;
                    }

                    var v = diagonalDown;
                    if (v > -1)
                    {
                        forbiddenLists[j].Add(v);
                        diagonalDown--;
                    }
                }
            }
        }
    }
}