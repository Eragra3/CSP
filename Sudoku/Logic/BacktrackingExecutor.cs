using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Models;

namespace Sudoku.Logic
{
    public static class BacktrackingExecutor
    {
        public static CSPSolution FindSolution(
            int[] variablesValues,
            Configuration.ValuePickingHeuristicsEnum valuePickingHeuristic,
            Configuration.VariablePickingHeuristicsEnum variablePickingHeuristic,
            Func<int[], int[], bool> conflictsFunc,
            int[] domain)
        {
            var backtracksCount = 0;

            var variablesCount = variablesValues.Length;

            var emptyVariablesIndeces = new HashSet<int>();
            for (var i = 0; i < variablesValues.Length; i++)
            {
                if (variablesValues[i] == -1)
                {
                    emptyVariablesIndeces.Add(i);
                }
            }
            var variablesWithoutValues = emptyVariablesIndeces.ToArray();

            int[] variableEvaluationOrder;
            switch (variablePickingHeuristic)
            {
                case Configuration.VariablePickingHeuristicsEnum.Increment:
                    variableEvaluationOrder = SudokuHelperMethods.GetIncrementalVariableOrder(variablesWithoutValues);
                    break;
                case Configuration.VariablePickingHeuristicsEnum.Random:
                    variableEvaluationOrder = SudokuHelperMethods.GetRandomVariableOrder(variablesWithoutValues);
                    break;
                default:
                    throw new Exception($"{variablePickingHeuristic} is not value of {nameof(Configuration.VariablePickingHeuristicsEnum)}");
            }


            var backtrackedValuesLists = new IList<int>[variablesCount];
            for (var i = 0; i < backtrackedValuesLists.Length; i++)
            {
                backtrackedValuesLists[i] = new List<int>(variablesCount);
            }

            var conflictingValues = new List<int>();

            for (var index = 0; index < variableEvaluationOrder.Length; index++)
            {
                var currentVariableValue = -1;
                var currentVariableIndex = variableEvaluationOrder[index];

                conflictingValues.Clear();

                var noValidValueInDomain = false;

                var domainWithoutObviousConflicts = domain
                                .Except(backtrackedValuesLists[currentVariableIndex])
                                .ToList();

                while (variablesValues[currentVariableIndex] == -1)
                {
                    switch (valuePickingHeuristic)
                    {
                        case Configuration.ValuePickingHeuristicsEnum.Increment:
                            currentVariableValue = SudokuHelperMethods.GetMinimumValue(
                                domainWithoutObviousConflicts
                                .Except(conflictingValues)
                                .ToList());
                            noValidValueInDomain = currentVariableValue == -1;
                            break;
                        case Configuration.ValuePickingHeuristicsEnum.Random:
                            var possibleRows = domainWithoutObviousConflicts.Except(conflictingValues).ToList();
                            if (possibleRows.Count == 0)
                                noValidValueInDomain = true;
                            else
                                currentVariableValue = SudokuHelperMethods.GetRandomValue(possibleRows);
                            break;
                        default:
                            throw new Exception($"Not existing value picking heuristic - {valuePickingHeuristic}");
                    }

                    if (noValidValueInDomain)
                    {
                        variablesValues[currentVariableIndex] = -1;
                        break;
                    }

                    variablesValues[currentVariableIndex] = currentVariableValue;
                    var conflicts = conflictsFunc(variablesValues, domain);


                    if (conflicts)
                    {
                        variablesValues[currentVariableIndex] = -1;
                        conflictingValues.Add(currentVariableValue);
                    }
                }

                //go back
                if (variablesValues[currentVariableIndex] == -1)
                {
                    if (index - 1 < 0)
                    {
                        return new CSPSolution(variablesValues, backtracksCount, variablesWithoutValues.Length, (int)Math.Sqrt(variablesCount));
                    }
                    backtracksCount++;
                    backtrackedValuesLists[currentVariableIndex].Clear();

                    index--;

                    //previous variable cannot use this value
                    var prevIndex = variableEvaluationOrder[index];
                    backtrackedValuesLists[prevIndex].Add(variablesValues[prevIndex]);
                    variablesValues[prevIndex] = -1;

                    //quickfix, because for will increment index
                    index--;
                }
            }

            var solution = new CSPSolution(variablesValues, backtracksCount, variablesWithoutValues.Length, (int)Math.Sqrt(variablesCount));

            return solution;
        }
    }
}
