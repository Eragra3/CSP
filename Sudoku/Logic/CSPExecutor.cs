using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sudoku.Models;

namespace Sudoku.Logic
{
    public static class CSPExecutor
    {
        //public static IEnumerable<SingleRunResultStatistics> RunExperiment(ConfigurationBatchFile config)
        //{
        //    for (int currentN = config.MinHoles; currentN < config.MaxHoles + 1; currentN++)
        //    {
        //        yield return FindAllSolutions(
        //            currentN,
        //            config.ValuePickingHeuristicMethod,
        //            config.VariablePickingHeuristicMethod,
        //            config.UsedExecutor);
        //    }
        //}

        public static IEnumerable<SingleRunResultStatistics> RunExperimentRepeat(ConfigurationBatchFile config,
            int repetitions, int[][] sudokuSolution)
        {
            for (var currentN = config.MinHoles; currentN < config.MaxHoles + 1; currentN++)
            {
                var run = new SingleRunResultStatistics(config.BoardSize, config.UsedExecutor, currentN);
                var sudokuModel = SudokuHelperMethods.RemoveRandomFields(sudokuSolution, currentN);
                int[] sudoku = new int[sudokuModel.Sudoku.Length];

                for (var i = 0; i < repetitions; i++)
                {
                    sudokuModel.Sudoku.CopyTo(sudoku, 0);
                    CSPSolution s;
                    switch (config.UsedExecutor)
                    {
                        case Configuration.ExecutorsEnum.Backtracking:
                            s = BacktrackingExecutor.FindSolution(
                                sudoku,
                                config.ValuePickingHeuristicMethod,
                                config.VariablePickingHeuristicMethod,
                                SudokuHelperMethods.Conflicts,
                                SudokuHelperMethods.GetDomain(config.BoardSize));
                            break;
                        case Configuration.ExecutorsEnum.ForwardChecking:
                            s = ForwardCheckingExecutor.FindSolution(
                                sudoku,
                                config.ValuePickingHeuristicMethod,
                                config.VariablePickingHeuristicMethod,
                                SudokuHelperMethods.Conflicts,
                            SudokuHelperMethods.GetDomain(config.BoardSize));
                            break;
                        default:
                            throw new Exception($"No such executor {config.UsedExecutor}");
                    }

                    run.AllRuns.Add(new SolutionStatistics(s.BacktracksCount, s.HolesCount, s.BoardSize));
                }

                yield return run;
            }
        }

        //public static SingleRunResultStatistics FindAllSolutions(
        //    int n,
        //    Configuration.ValuePickingHeuristicsEnum valuePickingMethod,
        //    Configuration.VariablePickingHeuristicsEnum variablePickingMethod,
        //    Configuration.ExecutorsEnum usedExecutor)
        //{
        //    var result = new SingleRunResultStatistics(n, usedExecutor);
        //    var sw = new Stopwatch();
        //    sw.Start();

        //    while (result.SolutionsCount < Configuration.AllSolutionsCount[n] &&
        //        sw.Elapsed.TotalSeconds < Configuration.MaxExperimentTimeInSeconds)
        //    {
        //        CSPSolution solution;

        //        switch (usedExecutor)
        //        {
        //            case Configuration.ExecutorsEnum.Backtracking:
        //                solution = BacktrackingExecutor.FindSolution(
        //                    n,
        //                    valuePickingMethod,
        //                    variablePickingMethod,
        //                    SudokuHelperMethods.SudokuCheckForConflicts,
        //                    Configuration.SudokuDomain);
        //                break;
        //            case Configuration.ExecutorsEnum.ForwardChecking:
        //                solution = ForwardCheckingExecutor.FindSolution(
        //                    n,
        //                    valuePickingMethod,
        //                    variablePickingMethod,
        //                    SudokuHelperMethods.SudokuCheckForConflicts,
        //                    SudokuHelperMethods.GetSudokuDomain(n));
        //                break;
        //            default:
        //                throw new Exception($"Not existing executor - {usedExecutor}");
        //        }


        //        result.AllRuns.Add(new SolutionStatistics(solution.Solution, solution.backtracksCount, n));
        //        if (!SudokuHelperMethods.Contains(result.AllSolutions, solution.Solution))
        //        {
        //            result.AllSolutions.Add(solution.Solution);
        //        }
        //    }

        //    return result;
        //}
    }
}
