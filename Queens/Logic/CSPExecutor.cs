using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queens.Models;
using Queens.Logic;

namespace Queens.Logic
{
    public static class CSPExecutor
    {
        public static IEnumerable<SingleRunResultStatistics> RunExperiment(ConfigurationBatchFile config)
        {
            for (int currentN = config.MinN; currentN < config.MaxN + 1; currentN++)
            {
                yield return FindAllSolutions(
                    currentN,
                    config.ValuePickingHeuristicMethod,
                    config.VariablePickingHeuristicMethod,
                    config.UsedExecutor);
            }
        }

        public static SingleRunResultStatistics FindAllSolutions(
            int n,
            ValuePickingHeuristicsEnum valuePickingMethod,
            VariablePickingHeuristicsEnum variablePickingMethod,
            ExecutorsEnum usedExecutor)
        {
            var result = new SingleRunResultStatistics(n, usedExecutor);
            var rand = new Random();
            var sw = new Stopwatch();
            sw.Start();

            while (result.SolutionsCount < Configuration.AllSolutionsCount[n] && sw.Elapsed.TotalSeconds < 60)
            {
                CSPSolution solution;

                switch (usedExecutor)
                {
                    case ExecutorsEnum.Backtracking:
                        solution = BacktrackingExecutor.FindSolution(
                    n,
                    valuePickingMethod,
                    variablePickingMethod,
                    QueensHelperMethods.QueensCheckForConflicts,
                    Configuration.QueensDomain);
                        break;
                    case ExecutorsEnum.ForwardChecking:
                        solution = ForwardCheckingExecutor.FindSolution(
                    n,
                    valuePickingMethod,
                    variablePickingMethod,
                    QueensHelperMethods.QueensCheckForConflicts,
                    Configuration.QueensDomain);
                        break;
                    default:
                        throw new Exception($"Not existing executor - {usedExecutor}");
                }


                result.AllRuns.Add(new SolutionStatistics(solution.Solution, solution.NumberOfBacktracks, n));
                if (!QueensHelperMethods.Contains(result.AllSolutions, solution.Solution))
                {
                    result.AllSolutions.Add(solution.Solution);
                }
            }

            return result;
        }
    }
}
