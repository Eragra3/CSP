using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sudoku.Logic;
using Sudoku.Models;
using static Sudoku.Configuration;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [ThreadStatic]
        private static Random _random;

        private static Random Random => _random ?? (_random = new Random());

        private Tile[][] Board { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SubBoardSizeTextBox.Text = Configuration.SUBBOARD_SIZE.ToString();
            HolesCountTextBox.Text = Configuration.HOLES_COUNT.ToString();
            RedrawBoard(Configuration.BOARD_SIZE);
        }

        private void RedrawBoard(int boardSize)
        {
            //cleanup
            BoardPanel.Children.Clear();
            BoardPanel.RowDefinitions.Clear();
            BoardPanel.ColumnDefinitions.Clear();

            var subBoardSize = Math.Sqrt(boardSize);

            //initialize board array
            Board = new Tile[boardSize][];
            for (var i = 0; i < Board.Length; i++)
            {
                Board[i] = new Tile[boardSize];
            }

            //create column definitions (partition board into subboards)
            for (var i = 0; i < subBoardSize; i++)
            {
                var colDef = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };

                var rowDef = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };

                BoardPanel.ColumnDefinitions.Add(colDef);
                BoardPanel.RowDefinitions.Add(rowDef);
            }

            //create subboards
            for (var i = 0; i < subBoardSize; i++)
            {
                for (var j = 0; j < subBoardSize; j++)
                {
                    var subBoard = new Grid();
                    subBoard.SetValue(Grid.RowProperty, i);
                    subBoard.SetValue(Grid.ColumnProperty, j);

                    //divide subboard into tiles
                    for (var n = 0; n < subBoardSize; n++)
                    {
                        var colDef = new ColumnDefinition
                        {
                            Width = new GridLength(1, GridUnitType.Star)
                        };

                        var rowDef = new RowDefinition
                        {
                            Height = new GridLength(1, GridUnitType.Star)
                        };

                        subBoard.ColumnDefinitions.Add(colDef);
                        subBoard.RowDefinitions.Add(rowDef);
                    }

                    //create tiles for subboard
                    for (var k = 0; k < subBoardSize; k++)
                    {
                        for (var l = 0; l < subBoardSize; l++)
                        {
                            var tile = new Tile();

                            if ((i + j) % 2 == 0)
                                tile.ContentTextBox.Background = Brushes.DeepSkyBlue;

                            tile.SetValue(Grid.RowProperty, k);
                            tile.SetValue(Grid.ColumnProperty, l);

                            switch (boardSize)
                            {
                                case 9:
                                    tile.ContentTextBox.FontSize = 26;
                                    break;
                                case 16:
                                    tile.ContentTextBox.FontSize = 20;
                                    break;
                                default:
                                    tile.ContentTextBox.FontSize = 12;
                                    break;
                            }

                            subBoard.Children.Add(tile);
                            var tileRowIndex = (int)(i * subBoardSize) + k;
                            var tileColumnIndex = (int)(j * subBoardSize) + l;

                            tile.ContentTextBox.TextChanged += RevalidateSolution;

                            Board[tileRowIndex][tileColumnIndex] = tile;
                        }
                    }

                    //add subboard to main board
                    BoardPanel.Children.Add(subBoard);
                }
            }
        }

        private void GenerateSudokuButtonClick(object sender, RoutedEventArgs e)
        {
            var boardSize = GetBoardSize();
            var holesCount = GetHolesCount();

            if (Board == null || !Board.Any() || Board.Length != boardSize)
            {
                RedrawBoard(boardSize);
            }
            RenderRandomSudoku(boardSize, holesCount);
        }



        private void RenderRandomSudoku(int boardSize, int fieldsToRemoveCount)
        {
            int[][] randomSolution;

            switch (boardSize)
            {
                case 9:
                    randomSolution = Configuration.GetRandom3x3Sudoku;
                    break;
                default:
                    randomSolution = null;
                    break;
            }

            if (randomSolution == null) return;

            var unfinishedSudoku = SudokuHelperMethods.RemoveRandomFields(randomSolution, fieldsToRemoveCount);

            DrawSudoku(unfinishedSudoku.Sudoku, boardSize);
        }


        private void RevalidateSolution(object sender, TextChangedEventArgs eventArgs)
        {
            RevalidateSolution();
        }

        private void RevalidateSolution()
        {
            var s2 = Board.SelectMany(element => element.Select(el => ParseInteger(el.ContentTextBox.Text))).ToArray();
            var r2 = SudokuHelperMethods.IsValid(s2, Configuration.SudokuDomain);

            if (r2)
            {
                BoardPanel.Background = Brushes.White;
            }
            else
            {
                BoardPanel.Background = Brushes.Red;
            }
        }

        private void BoardPanel_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private int ParseInteger(string text)
        {
            int result;

            if (string.IsNullOrEmpty(text) || !int.TryParse(text, out result))
            {
                result = -1;
            }

            return result;
        }

        private void BacktrackingButtonClick(object sender, RoutedEventArgs e)
        {
            var valuePH = GetValuePickingHeuristic();
            var variablePH = GetVariablePickingHeuristic();
            var boardSize = GetBoardSize();
            var holesCount = GetHolesCount();

            var sudoku = GetSudokuFromBoard();

            var solution = BacktrackingExecutor.FindSolution(
                sudoku,
                valuePH,
                variablePH,
                SudokuHelperMethods.Conflicts,
                SudokuHelperMethods.GetDomain(boardSize));

            if (solution.Solution != null)
            {
                DrawSudoku(solution.Solution, boardSize);
            }
            ShowStatisticsWindow(solution);
        }

        private void DrawSudoku(int[] sudoku, int boardSize)
        {
            for (var i = 0; i < Board.Length; i++)
            {
                var row = Board[i];
                for (var j = 0; j < row.Length; j++)
                {
                    var v = sudoku[(i * boardSize) + j];
                    row[j].ContentTextBox.Text = v == -1 ? string.Empty : v.ToString();
                }
            }
        }


        private void ShowStatisticsWindow(CSPSolution solution)
        {
            var window = new StatisicsWindow
            {
                NumberOfBacktracksLabel =
                {
                    Content = Regex.Replace(
                        solution?.BacktracksCount
                        .ToString()
                        .Reverse(),
                        @"(.{3})", "$1 ")
                        .TrimEnd()
                        .Reverse()
                }
            };
            window.NoSolutionLabel.Visibility = solution?.Solution == null ? Visibility.Visible : Visibility.Hidden;
            window.Show();
        }

        private void ClearSudokuFields()
        {
            foreach (var row in Board)
            {
                foreach (var tile in row)
                {
                    tile.ContentTextBox.Text = string.Empty;
                }
            }
        }

        #region variable getters
        private int GetBoardSize()
        {
            int boardSize;
            if (int.TryParse(SubBoardSizeTextBox.Text, out boardSize))
            {
                boardSize *= boardSize;
            }
            else
            {
                boardSize = Configuration.BOARD_SIZE;
            }

            Configuration.GuiBoardSize = boardSize;

            return boardSize;
        }

        private int GetHolesCount()
        {
            int holesCount;

            if (!int.TryParse(HolesCountTextBox.Text, out holesCount))
            {
                holesCount = Configuration.HOLES_COUNT;
            }

            return holesCount;
        }
        private VariablePickingHeuristicsEnum GetVariablePickingHeuristic()
        {
            VariablePickingHeuristicsEnum vph;
            try
            {
                vph = (VariablePickingHeuristicsEnum)
                    Enum.Parse(typeof(VariablePickingHeuristicsEnum), VariablePickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                vph = VariablePickingHeuristicsEnum.Random;
            }
            return vph;
        }

        private ValuePickingHeuristicsEnum GetValuePickingHeuristic()
        {
            ValuePickingHeuristicsEnum vph;
            try
            {
                vph = (ValuePickingHeuristicsEnum)
                    Enum.Parse(typeof(ValuePickingHeuristicsEnum), ValuePickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                vph = ValuePickingHeuristicsEnum.Random;
            }
            return vph;
        }

        private int[] GetSudokuFromBoard()
        {
            return Board.SelectMany(element => element.Select(el => ParseInteger(el.ContentTextBox.Text))).ToArray();
        }

        #endregion

        private void ForwardCheckingButtonClick(object sender, RoutedEventArgs e)
        {
            var valuePH = GetValuePickingHeuristic();
            var variablePH = GetVariablePickingHeuristic();
            var boardSize = GetBoardSize();
            var holesCount = GetHolesCount();

            var sudoku = GetSudokuFromBoard();

            var solution = ForwardCheckingExecutor.FindSolution(
                sudoku,
                valuePH,
                variablePH,
                SudokuHelperMethods.Conflicts,
                SudokuHelperMethods.GetDomain(boardSize));

            if (solution.Solution != null)
            {
                DrawSudoku(solution.Solution, boardSize);
            }
            ShowStatisticsWindow(solution);
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            ClearSudokuFields();
        }

        private void AdvancedButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new AdvancedWindow();

            window.Show();
        }

        private void RemoveFieldsClick(object sender, RoutedEventArgs e)
        {
            var boardSize = GetBoardSize();
            var removedFieldsCount = GetHolesCount();

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

            var unfinishedSudoku = Board.SelectMany(row => row).ToArray();
            foreach (var removedFieldIndex in removedFields)
            {
                unfinishedSudoku[removedFieldIndex].ContentTextBox.Text = string.Empty;
            }

        }
    }
}
