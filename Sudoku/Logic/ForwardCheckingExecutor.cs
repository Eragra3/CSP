using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Models;

namespace Sudoku.Logic
{
    public static class ForwardCheckingExecutor
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

            var invalidValuesLists = new HashSet<int>[variablesCount];
            var backtrackedValuesLists = new HashSet<int>[variablesCount];
            for (var i = 0; i < invalidValuesLists.Length; i++)
            {
                invalidValuesLists[i] = new HashSet<int>();
                backtrackedValuesLists[i] = new HashSet<int>();
            }
            var valuesMakingOtherDomainsEmpty = new HashSet<int>();

            UpdateValidValues(variablesValues, invalidValuesLists, domain);

            for (var index = 0; index < variableEvaluationOrder.Length; index++)
            {
                var currentVariableValue = -1;
                var currentVariableIndex = variableEvaluationOrder[index];

                var validValues = domain
                    .Except(invalidValuesLists[currentVariableIndex])
                    .Except(backtrackedValuesLists[currentVariableIndex])
                    .Except(valuesMakingOtherDomainsEmpty)
                    .ToList();


                if (validValues.Count == 0)
                {
                    currentVariableValue = -1;
                }
                else
                {
                    switch (valuePickingHeuristic)
                    {
                        case Configuration.ValuePickingHeuristicsEnum.Increment:
                            currentVariableValue = SudokuHelperMethods.GetMinimumValue(validValues);
                            break;
                        case Configuration.ValuePickingHeuristicsEnum.Random:
                            currentVariableValue = SudokuHelperMethods.GetRandomValue(validValues);
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
                //go back only if you cant pick other value for current variable
                //(there was only 1 valid value and it failed)
                if (currentVariableValue == -1 ||
                    UnassignedVariableHasEmptyDomain(
                        invalidValuesLists,
                        variablesValues,
                        backtrackedValuesLists,
                        domain))
                {
                    backtracksCount++;
                    //if there are other possible values for this variables, try them
                    if (validValues.Count > 1)
                    {
                        index--;
                        variablesValues[currentVariableIndex] = -1;
                        valuesMakingOtherDomainsEmpty.Add(currentVariableValue);
                        UpdateValidValues(variablesValues, invalidValuesLists, domain);
                    }
                    else
                    {
                        valuesMakingOtherDomainsEmpty.Clear();

                        if (index - 1 < 0)
                        {
                            return new CSPSolution(variablesValues, backtracksCount, variablesCount, variablesWithoutValues.Length);
                        }

                        variablesValues[currentVariableIndex] = -1;

                        backtrackedValuesLists[currentVariableIndex].Clear();

                        index--;

                        var previousVariableIndex = variableEvaluationOrder[index];


                        backtrackedValuesLists[previousVariableIndex]
                            .Add(variablesValues[previousVariableIndex]);
                        variablesValues[previousVariableIndex] = -1;

                        UpdateValidValues(variablesValues, invalidValuesLists, domain);

                        index--;
                    }
                }
                else
                {
                    valuesMakingOtherDomainsEmpty.Clear();
                }
            }

            var solution = new CSPSolution(variablesValues, backtracksCount, variablesCount, variablesWithoutValues.Length);
            return solution;
        }

        private static void UpdateValidValues(
            int[] variablesValues,
            HashSet<int>[] invalidValuesLists,
            int[] domain)
        {
            foreach (var iv in invalidValuesLists)
            {
                iv.Clear();
            }
            //delete from rows
            var boardSize = domain.Length;
            var rowOffset = -boardSize;
            for (var i = 0; i < variablesValues.Length; i++)
            {
                if (i % boardSize == 0) rowOffset += boardSize;

                var usedValue = variablesValues[i];
                if (usedValue == -1) continue;

                for (var columnIndex = 0; columnIndex < boardSize; columnIndex++)
                {
                    invalidValuesLists[rowOffset + columnIndex].Add(usedValue);
                }
            }

            //delete from column
            for (var i = 0; i < variablesValues.Length; i++)
            {
                var usedValue = variablesValues[i];
                if (usedValue == -1) continue;

                var columnOffset = i % boardSize;
                for (var rowIndex = 0; rowIndex < boardSize; rowIndex++)
                {
                    invalidValuesLists[columnOffset + (rowIndex * boardSize)].Add(usedValue);
                }

            }

            //delete from subgrid
            var subBoardSize = (int)Math.Sqrt(boardSize);
            var subGridRowOffset = 0;
            for (var subGridIndex = 0; subGridIndex < boardSize; subGridIndex++)
            {
                //read subgrid rows
                //foreach row
                for (var i = 0; i < subBoardSize; i++)
                {
                    //foreach column
                    for (var j = 0; j < subBoardSize; j++)
                    {
                        var usedValue = variablesValues[subGridRowOffset + (subGridIndex * subBoardSize) + (i * boardSize) + j];
                        if (usedValue == -1) continue;

                        //again foreach cell in this subgrid
                        for (var k = 0; k < subBoardSize; k++)
                        {
                            for (var n = 0; n < subBoardSize; n++)
                            {
                                invalidValuesLists[subGridRowOffset + (subGridIndex * subBoardSize) + (k * boardSize) + n]
                                    .Add(usedValue);
                            }
                        }
                    }
                }
                if ((subGridIndex + 1) % subBoardSize == 0)
                {
                    subGridRowOffset += (subBoardSize - 1) * boardSize;
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

            for (var i = 0; !hasEmptyDomain && i < variablesValues.Length; i++)
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