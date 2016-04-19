using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Queens.Models;

namespace Queens.Logic
{
    public class ForwardCheckingExecutor
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

            var invalidValuesLists = new HashSet<int>[n];
            var backtrackedValuesLists = new HashSet<int>[n];
            for (var i = 0; i < invalidValuesLists.Length; i++)
            {
                invalidValuesLists[i] = new HashSet<int>();
                backtrackedValuesLists[i] = new HashSet<int>();
            }

            var allRows = new List<int>();
            for (var i = 0; i < n; i++)
            {
                allRows.Add(i);
            }

            for (var index = 0; index < variableEvaluationOrder.Length; index++)
            {
                var currentVariableValue = -1;
                var currentVariableIndex = variableEvaluationOrder[index];

                var validValues = allRows
                    .Except(invalidValuesLists[currentVariableIndex])
                    .Except(backtrackedValuesLists[currentVariableIndex])
                    .ToList();


                if (validValues.Count == 0)
                {
                    currentVariableValue = -1;
                }
                else
                {
                    switch (valuePickingHeuristic)
                    {
                        case ValuePickingHeuristicsEnum.Increment:
                            currentVariableValue = QueensHelperMethods.GetMinimumValue(validValues);
                            break;
                        case ValuePickingHeuristicsEnum.Random:
                            currentVariableValue = QueensHelperMethods.GetRandomValue(validValues);
                            break;
                        default:
                            throw new Exception($"Not existing value picking heuristic - {valuePickingHeuristic}");
                    }
                }

                if (currentVariableValue != -1)
                {
                    variablesValues[currentVariableIndex] = currentVariableValue;
                    UpdateValidValues(variablesValues, invalidValuesLists, domain);
                }
                //go back if no valid row or not assigned queens have no possible rows
                if (currentVariableValue == -1 ||
                    UnassignedVariableHasEmptyDomain(
                        invalidValuesLists,
                        variablesValues,
                        backtrackedValuesLists,
                        domain))
                {
                    variablesValues[currentVariableIndex] = -1;
                    backtracksCount++;

                    backtrackedValuesLists[currentVariableIndex].Clear();

                    index--;

                    if (index < 0)
                    {
                        return new CSPSolution(null, backtracksCount, n);
                    }

                    var previousVariableIndex = variableEvaluationOrder[index];


                    backtrackedValuesLists[previousVariableIndex]
                        .Add(variablesValues[previousVariableIndex]);
                    variablesValues[previousVariableIndex] = -1;

                    UpdateValidValues(variablesValues, invalidValuesLists, domain);

                    index--;
                }
            }

            var solution = new CSPSolution(variablesValues, backtracksCount, n);
            return solution;
        }

        private static void UpdateValidValues(
            int[] variablesValues,
            HashSet<int>[] invalidValuesLists, 
            int[] domain)
        {
            foreach (var vv in invalidValuesLists)
            {
                vv.Clear();
            }

            for (int i = 0; i < variablesValues.Length; i++)
            {
                var usedValue = variablesValues[i];
                if (usedValue == -1) continue;


                for (int j = 0; j < invalidValuesLists.Length; j++)
                {
                    var horizontal = usedValue;
                    var diagonal1 = usedValue + Math.Abs(i - j);
                    var diagonal2 = usedValue - Math.Abs(i - j);

                    var invalidValuesHashSet = invalidValuesLists[j];
                    //horizontal
                    invalidValuesHashSet.Add(horizontal);
                    if (domain.Contains(diagonal1))
                    {
                        invalidValuesHashSet.Add(diagonal1);
                    }
                    if (domain.Contains(diagonal2))
                    {
                        invalidValuesHashSet.Add(diagonal2);
                    }
                }
            }
        }

        private static bool UnassignedVariableHasEmptyDomain(
            HashSet<int>[] invalidValuesLists,
            int[] variablesValues,
            HashSet<int>[] backtrackedValuesLists,
            int[] domain)
        {
            var hasEmptyDomain = false;

            for (int i = 0; !hasEmptyDomain && i < variablesValues.Length; i++)
            {
                if (variablesValues[i] != -1) continue;

                hasEmptyDomain = invalidValuesLists[i]
                    .Union(backtrackedValuesLists[i])
                    .Count() == domain.Length;
            }

            return hasEmptyDomain;
        }
    }
}