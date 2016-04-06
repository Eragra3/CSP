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
                solutionRows[i] = -1;
            }

            var backtrackedRowsLists = new IList<int>[n];
            for (var i = 0; i < backtrackedRowsLists.Length; i++)
            {
                backtrackedRowsLists[i] = new List<int>(n);
            }

            var allRows = new List<int>();
            for (var i = 0; i < n; i++)
            {
                allRows.Add(i);
            }

            var conflictingRows = new List<int>();

            for (var columnIndex = 0; columnIndex < solutionRows.Length; columnIndex++)
            {
                var rowIndex = -1;
                conflictingRows.Clear();

                var noRowFound = false;

                var removedObviousConflicts = allRows
                                .Except(backtrackedRowsLists[columnIndex])
                                .Except(solutionRows).ToList();

                while (solutionRows[columnIndex] == -1)
                {
                    switch (rowPickingHeuristic)
                    {
                        case RowPickingHeuristicsEnum.Increment:
                            rowIndex = QueensHelperMethods.GetNextPossibleRow(
                                removedObviousConflicts
                                .Except(conflictingRows)
                                .ToList());
                            noRowFound = rowIndex == -1;
                            break;
                        case RowPickingHeuristicsEnum.Random:
                            var possibleRows = removedObviousConflicts.Except(conflictingRows).ToList();
                            if (possibleRows.Count == 0)
                                noRowFound = true;
                            else
                                rowIndex = QueensHelperMethods.GetRandomRow(possibleRows);
                            break;
                        default:
                            break;
                    }
                    if (noRowFound)
                    {
                        solutionRows[columnIndex] = -1;
                        break;
                    }

                    var conflicts = false;
                    for (var k = 0; !conflicts && k < solutionRows.Length; k++)
                    {
                        if (k == columnIndex || solutionRows[k] == -1) continue;

                        conflicts = QueensHelperMethods.Conflicts(k, solutionRows[k], columnIndex, rowIndex);
                    }

                    if (conflicts)
                    {
                        conflictingRows.Add(rowIndex);
                    }
                    else
                    {
                        solutionRows[columnIndex] = rowIndex;
                    }
                }

                //go back
                if (solutionRows[columnIndex] == -1)
                {
                    numberOfBacktracks++;
                    backtrackedRowsLists[columnIndex].Clear();

                    columnIndex--;

                    if (columnIndex < 0)
                    {
                        return new CSPSolution(solutionRows, numberOfBacktracks, n);
                    }

                    backtrackedRowsLists[columnIndex].Add(solutionRows[columnIndex]);
                    solutionRows[columnIndex] = -1;

                    columnIndex--;
                }

            }

            var solution = new CSPSolution(solutionRows, numberOfBacktracks, n);

            return solution;
        }

        public static IEnumerable<RunResultStatistics> RunExperiment(ConfigurationBatchFile config)
        {
            for (int currentN = 1; currentN < config.MaxN + 1; currentN++)
            {
                var result = new RunResultStatistics(currentN);

                yield return FindAllSolutions(currentN, config.RowPickingHeuristicMethod, config.QueenPickingHeuristicMethod);

            }
        }

        public static RunResultStatistics FindAllSolutions(
            int n,
            RowPickingHeuristicsEnum rowPickingMethod,
            QueenPickingHeuristicsEnum queenPickingMethod)
        {
            var result = new RunResultStatistics(n);

            //while (result.AllRunsWithNewSolution.Count != n)
            //{

            //}

            var solution = FindSolution(n, rowPickingMethod);

            result.AllRunsWithNewSolution.Add(new SolutionStatistics(solution.Solution, solution.NumberOfBacktracks, n, 1));

            return result;
        }
    }
}
