using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Queens.Models;

namespace Queens.Logic
{
    public static class BacktrackingExecutor
    {
        public static CSPSolution FindSolution(
            int n,
            ValuePickingHeuristicsEnum valuePickingHeuristic,
            VariablePickingHeuristicsEnum variablePickingHeuristic,
            Func<int[], int, int, int[], bool> conflictsFunc,
            int[] domain)
        {
            var backtracksCount = 0;

            var variablesValues = new int[n];
            for (var i = 0; i < variablesValues.Length; i++)
            {
                variablesValues[i] = -1;
            }

            int[] variableEvaluationOrder;
            switch (variablePickingHeuristic)
            {
                case VariablePickingHeuristicsEnum.Increment:
                    variableEvaluationOrder = QueensHelperMethods.GetIncrementalVariableOrder(n);
                    break;
                case VariablePickingHeuristicsEnum.Random:
                    variableEvaluationOrder = QueensHelperMethods.GetRandomVariableOrder(n);
                    break;
                default:
                    throw new Exception($"{variablePickingHeuristic} is not value of {nameof(VariablePickingHeuristicsEnum)}");
            }


            var backtrackedValuesLists = new IList<int>[n];
            for (var i = 0; i < backtrackedValuesLists.Length; i++)
            {
                backtrackedValuesLists[i] = new List<int>(n);
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
                                .Except(variablesValues).ToList();

                while (variablesValues[currentVariableIndex] == -1)
                {
                    switch (valuePickingHeuristic)
                    {
                        case ValuePickingHeuristicsEnum.Increment:
                            currentVariableValue = QueensHelperMethods.GetMinimumValue(
                                domainWithoutObviousConflicts
                                .Except(conflictingValues)
                                .ToList());
                            noValidValueInDomain = currentVariableValue == -1;
                            break;
                        case ValuePickingHeuristicsEnum.Random:
                            var possibleRows = domainWithoutObviousConflicts.Except(conflictingValues).ToList();
                            if (possibleRows.Count == 0)
                                noValidValueInDomain = true;
                            else
                                currentVariableValue = QueensHelperMethods.GetRandomValue(possibleRows);
                            break;
                        default:
                            throw new Exception($"Not existing value picking heuristic - {valuePickingHeuristic}");
                    }

                    if (noValidValueInDomain)
                    {
                        variablesValues[currentVariableIndex] = -1;
                        break;
                    }

                    var conflicts = conflictsFunc(variablesValues, currentVariableValue, currentVariableIndex, domain);


                    if (conflicts)
                    {
                        conflictingValues.Add(currentVariableValue);
                    }
                    else
                    {
                        variablesValues[currentVariableIndex] = currentVariableValue;
                    }
                }

                //go back
                if (variablesValues[currentVariableIndex] == -1)
                {
                    backtracksCount++;
                    backtrackedValuesLists[currentVariableIndex].Clear();

                    index--;
                    if (index < 0)
                    {
                        return new CSPSolution(variablesValues, backtracksCount, n);
                    }

                    //previous variable cannot use this value
                    var prevIndex = variableEvaluationOrder[index];
                    backtrackedValuesLists[prevIndex].Add(variablesValues[prevIndex]);
                    variablesValues[prevIndex] = -1;

                    //quickfix, because for will increment index
                    index--;
                }
            }

            var solution = new CSPSolution(variablesValues, backtracksCount, n);

            return solution;
        }
    }
}
