using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Logic
{
    public static class SudokuHelperMethods
    {
        [ThreadStatic]
        private static Random _random;

        private static Random Random => _random ?? (_random = new Random());

        public static bool Conflicts(int[] solution, int[] domain)
        {
            var isValid = true;

            var boardSize = (int)Math.Sqrt(solution.Length);
            var subBoardSize = (int)Math.Sqrt(boardSize);

            var usedValues = new HashSet<int>();

            // check rows
            for (var i = 0; isValid && i < solution.Length; i++)
            {
                if (i % boardSize == 0)
                {
                    usedValues.Clear();
                }

                var value = solution[i];
                if (value == -1 || value == 0) continue;

                isValid = domain.Contains(value) && !usedValues.Contains(value);


                if (!isValid) continue;

                usedValues.Add(value);
            }

            // check columns
            for (var i = 0; isValid && i < boardSize; i++)
            {
                usedValues.Clear();
                for (var j = 0; isValid && j < boardSize; j++)
                {
                    var value = solution[i + (j * boardSize)];
                    if (value == -1 || value == 0) continue;

                    isValid = !usedValues.Contains(value);
                    if (!isValid) continue;

                    usedValues.Add(value);
                }
            }

            // check sub-grids
            //board
            var subGridRowOffset = 0;
            for (var subGridIndex = 0; isValid && subGridIndex < boardSize; subGridIndex++)
            {
                usedValues.Clear();
                //read subgrid rows
                //sub-grid
                for (var i = 0; isValid && i < subBoardSize; i++)
                {
                    //row
                    for (var j = 0; isValid && j < subBoardSize; j++)
                    {
                        var value = solution[subGridRowOffset + (subGridIndex * subBoardSize) + (i * boardSize) + j];
                        if (value == -1 || value == 0) continue;

                        isValid = !usedValues.Contains(value);
                        if (!isValid) continue;

                        usedValues.Add(value);
                    }
                }
                if ((subGridIndex + 1) % subBoardSize == 0)
                {
                    subGridRowOffset += (subBoardSize - 1) * boardSize;
                }
            }

            return !isValid;
        }

        public static bool IsValid(int[] solution, int[] domain)
        {
            var isValid = true;

            for (int i = 0; isValid && i < solution.Length; i++)
            {
                isValid = solution[i] != -1 && solution[i] != 0;
            }

            if (!isValid) return false;

            return !Conflicts(solution, domain);
        }

        public static SudokuModel RemoveRandomFields(int[][] sudoku, int removedFieldsCount)
        {
            var boardSize = sudoku.Length;

            var removedFields = new HashSet<int>();

            if (removedFieldsCount > boardSize * boardSize)
            {
                removedFieldsCount = Configuration.HOLES_COUNT;
            }

            while (removedFieldsCount > 0)
            {
                int index;
                do
                {
                    index = Random.Next(0, boardSize * boardSize);
                } while (removedFields.Contains(index));

                removedFields.Add(index);

                removedFieldsCount--;
            }

            var unfinishedSudoku = sudoku.SelectMany(row => row).ToArray();
            foreach (var removedFieldIndex in removedFields)
            {
                unfinishedSudoku[removedFieldIndex] = -1;
            }


            var result = new SudokuModel
            {
                Sudoku = unfinishedSudoku,
                RemovedFields = removedFields.ToArray()
            };

            return result;
        }

        public static int[] GetDomain(int boardSize)
        {
            var domain = new int[boardSize];
            var number = 1;
            for (var i = 0; i < boardSize; i++)
            {
                domain[i] = number++;
            }
            return domain;
        }

        public class SudokuModel
        {
            public int[] Sudoku;

            public int[] RemovedFields;
        }

        public static int GetRandomValue(List<int> possibleValues)
        {
            if (possibleValues.Count == 0) return -1;
            return possibleValues.ElementAt(Random.Next(0, possibleValues.Count));
        }

        /// <summary>
        /// return next possible int or -1
        /// </summary>
        /// <param name="possibleValues"></param>
        /// <returns></returns>
        public static int GetMinimumValue(List<int> possibleValues)
        {
            if (possibleValues.Count == 0) return -1;
            return possibleValues.Min();
        }

        public static int[] GetRandomVariableOrder(int[] variableIndeces)
        {
            return Random.Shuffle(variableIndeces);
        }

        public static int[] GetIncrementalVariableOrder(int[] variableIndeces)
        {
            Array.Sort(variableIndeces);
            return variableIndeces;
        }

    }
}
