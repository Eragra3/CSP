using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Queens.Logic
{
    public static class QueensHelperMethods
    {
        [ThreadStatic]
        private static Random _random;

        private static Random Random
        {
            get
            {
                if (_random == null)
                    _random = new Random();

                return _random;
            }
        }

        public static bool QueensCheckForConflicts(int[] allVariables, int currentValue, int currentIndex)
        {
            var conflicts = false;
            for (var k = 0; !conflicts && k < allVariables.Length; k++)
            {
                //do not check conflicts with itself and unnasigned variables
                if (k == currentIndex || allVariables[k] == -1) continue;

                conflicts = Conflicts(k, allVariables[k], currentIndex, currentValue);
            }
            return conflicts;
        }

        public static bool Contains(IList<int[]> solutions, int[] currentSolution)
        {
            if (currentSolution == null) return false;

            var contains = false;
            for (var i = 0; !contains && i < solutions.Count; i++)
            {
                var equals = true;
                for (var j = 0; equals && j < solutions[i].Length; j++)
                {
                    equals = solutions[i][j] == currentSolution[j];
                }
                contains = equals;
            }
            return contains;
        }

        public static bool Conflicts(int i1, int j1, int i2, int j2)
        {
            return i1 == i2 || j1 == j2 || Math.Abs(i1 - i2) == Math.Abs(j1 - j2);
        }

        public static int GetRandomValue(List<int> possibleValues)
        {
            if (possibleValues.Count == 0) return -1;
            return possibleValues.ElementAt(Random.Next(0, possibleValues.Count));
        }

        /// <summary>
        /// return next possible int or -1
        /// </summary>
        /// <param name="possibleRows"></param>
        /// <returns></returns>
        public static int GetMinimumValue(List<int> possibleRows)
        {
            if (possibleRows.Count == 0) return -1;
            return possibleRows.Min();
        }

        public static void AssertSolutionCorrect(int[] variablesValues)
        {
            if (variablesValues == null) return;
            for (var i1 = 0; i1 < variablesValues.Length; i1++)
            {
                var j1 = variablesValues[i1];
                for (var i2 = 0; i2 < variablesValues.Length; i2++)
                {
                    if (i2 == i1) continue;
                    var j2 = variablesValues[i2];
                    if (Conflicts(i1, j1, i2, j2))
                    {
                        throw new Exception("Solution is invalid!");
                    }
                }
            }
        }

        public static int[] GetRandomVariableOrder(int size)
        {
            return Enumerable.Range(0, size).OrderBy(x => Random.Next()).ToArray();
        }

        public static int[] GetIncrementalVariableOrder(int size)
        {
            return Enumerable.Range(0, size).ToArray();
        }

        public static int[] GetQueensDomain(int n)
        {
            var domain = new int[n];
            for (var i = 0; i < domain.Length; i++)
            {
                domain[i] = i;
            }

            return domain;
        }
    }
}
