using System;
using System.Collections.Generic;
using System.Configuration;
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
using Queens;
using Queens.Logic;
using Queens.Models;
using Queens.Extensions;

namespace Queens
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int BoardSize { get; set; } = Configuration.BoardSize;

        public Tile[][] Board { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            BoardSizeTextBox.Text = Configuration.BoardSize.ToString();

            RedrawBoard();
        }

        public void RedrawBoard()
        {
            //cleanup
            BoardPanel.Children.Clear();
            BoardPanel.RowDefinitions.Clear();
            BoardPanel.ColumnDefinitions.Clear();

            var boardSize = Configuration.BoardSize;

            //initialize board array
            Board = new Tile[boardSize][];
            for (var i = 0; i < Board.Length; i++)
            {
                Board[i] = new Tile[boardSize];
            }

            //create column definitions (partition board)
            for (var i = 0; i < boardSize; i++)
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

            //fill board with tiles
            for (var i = 0; i < Board.Length; i++)
            {
                for (var j = 0; j < Board[i].Length; j++)
                {
                    var tile = new Tile();
                    if ((j + i) % 2 == 0) tile.TileLabel.Background = Brushes.DeepSkyBlue;
                    tile.SetValue(Grid.ColumnProperty, i);
                    tile.SetValue(Grid.RowProperty, j);
                    if (boardSize < 14)
                    {
                        tile.TileLabel.FontSize = 26;
                    }
                    else if (boardSize < 20)
                    {
                        tile.TileLabel.FontSize = 16;
                    }
                    else if (BoardSize < 30)
                    {
                        tile.TileLabel.FontSize = 10;
                    }
                    else if (BoardSize < 35)
                    {
                        tile.TileLabel.FontSize = 4;
                    }
                    else
                    {
                        tile.TileLabel.FontSize = 0;
                    }

                    BoardPanel.Children.Add(tile);
                    Board[i][j] = tile;
                }
            }
        }

        public void RedrawBoard(int[] queensPositions)
        {
            RedrawBoard();

            if (!Configuration.RenderBoard) return;

            if (queensPositions == null)
            {
                NoSolutionLabel.Visibility = Visibility.Visible;
            }
            else
            {
                NoSolutionLabel.Visibility = Visibility.Hidden;
                for (var i = 0; i < Board.Length; i++)
                {
                    PutQueenOnTile(Board[i][queensPositions[i]]);
                }
            }
        }

        public void RedrawQueens(int[] queensPositions)
        {
            for (var i = 0; i < Board.Length; i++)
            {
                for (var j = 0; j < Board[i].Length; j++)
                {
                    if ((j + i) % 2 == 0) Board[i][j].TileLabel.Background = Brushes.DeepSkyBlue;
                }
            }

            for (var i = 0; i < Board.Length; i++)
            {
                PutQueenOnTile(Board[i][queensPositions[i]]);
            }
        }
        public void RemoveQueens()
        {
            for (var i = 0; i < Board.Length; i++)
            {
                for (var j = 0; j < Board[i].Length; j++)
                {
                    if ((j + i) % 2 == 0) Board[i][j].TileLabel.Background = Brushes.DeepSkyBlue;
                }
            }
        }

        public void PutQueenOnTile(Tile t)
        {
            t.TileLabel.Background = Brushes.Black;
        }

        private void BoardSizeTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsPositiveInteger(e.Text);
        }

        private static bool IsPositiveInteger(string text)
        {
            var regex = new Regex("[0-9]+");
            return !regex.IsMatch(text);
        }

        private async void ForwardCheckButton_Click(object sender, RoutedEventArgs e)
        {
            ReadValues();

            BlockControls();
            var solution = await Task.Factory.StartNew(() => ForwardCheckingExecutor.FindSolution(
                Configuration.BoardSize,
                Configuration.RowPickingHeuristic
                ));
            UnblockControls();

            QueensHelperMethods.AssertSolutionCorrect(solution.Solution);
            RedrawBoard(solution.Solution);
            ShowStatisticsWindow(solution);
        }

        private async void BacktrackingButton_Click(object sender, RoutedEventArgs e)
        {
            ReadValues();

            BlockControls();
            var solution = await Task.Factory.StartNew(() => BacktrackingExecutor.FindSolution(
                Configuration.BoardSize,
                Configuration.RowPickingHeuristic
                ));
            UnblockControls();

            QueensHelperMethods.AssertSolutionCorrect(solution.Solution);
            RedrawBoard(solution.Solution);
            ShowStatisticsWindow(solution);
        }

        private void ReadValues()
        {
            try
            {
                Configuration.BoardSize = int.Parse(BoardSizeTextBox.Text);
            }
            catch (Exception)
            {
                Configuration.BoardSize = 0;
            }
            try
            {
                Configuration.RowPickingHeuristic = (RowPickingHeuristicsEnum)
                    Enum.Parse(typeof(RowPickingHeuristicsEnum), RowPickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                Configuration.RowPickingHeuristic = RowPickingHeuristicsEnum.Increment;
            }
            try
            {
                Configuration.ShowStatisticsWindow = ShowStatisticsCheckBox.IsChecked ?? false;
            }
            catch (Exception)
            {
                Configuration.ShowStatisticsWindow = false;
            }
            try
            {
                Configuration.RenderBoard = RenderBoardCheckBox.IsChecked ?? false;
            }
            catch (Exception)
            {
                Configuration.RenderBoard = false;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ReadValues();

            BlockControls();
            RedrawBoard();
            UnblockControls();
        }

        private void ShowStatisticsWindow(CSPSolution solution)
        {
            if (Configuration.ShowStatisticsWindow)
            {
                var window = new StatisicsWindow
                {
                    NumberOfBacktracksLabel =
                {
                    Content = Regex.Replace(
                        solution?.NumberOfBacktracks
                        .ToString()
                        .Reverse(),
                        @"(.{3})", "$1 ")
                        .TrimEnd()
                        .Reverse()
                }
                };

                window.Show();
            }
        }

        private void BlockControls()
        {
            BacktrackingButton.IsEnabled = false;
            ForwardCheckButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
            ProcessingLabel.Visibility = Visibility.Visible;
        }

        private void UnblockControls()
        {
            BacktrackingButton.IsEnabled = true;
            ForwardCheckButton.IsEnabled = true;
            ClearButton.IsEnabled = true;
            ProcessingLabel.Visibility = Visibility.Hidden;
        }

        private void OpenAdvancedWindow(object sender, RoutedEventArgs e)
        {
            var window = new AdvancedWindow();

            window.Show();
        }
    }
}
