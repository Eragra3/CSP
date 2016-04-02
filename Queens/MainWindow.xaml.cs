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
using Sudoku.Models;

namespace Sudoku
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
                    if ((j + i) % 2 == 0) tile.MainStackPanel.Background = Brushes.DeepSkyBlue;
                    tile.SetValue(Grid.ColumnProperty, i);
                    tile.SetValue(Grid.RowProperty, j);

                    BoardPanel.Children.Add(tile);
                    Board[i][j] = tile;
                }
            }
        }

        public void RedrawBoard(int[] queensPositions)
        {
            RedrawBoard();

            for (var i = 0; i < Board.Length; i++)
            {
                PutQueenOnTile(Board[i][queensPositions[i]]);
            }
        }

        public void RedrawQueens(int[] queensPositions)
        {
            for (var i = 0; i < Board.Length; i++)
            {
                for (var j = 0; j < Board[i].Length; j++)
                {
                    if ((j + i) % 2 == 0) Board[i][j].MainStackPanel.Background = Brushes.DeepSkyBlue;
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
                    if ((j + i) % 2 == 0) Board[i][j].MainStackPanel.Background = Brushes.DeepSkyBlue;
                }
            }
        }

        public void PutQueenOnTile(Tile t)
        {
            t.MainStackPanel.Background = Brushes.Chartreuse;
        }

        private void BoardSizeTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsPositiveInteger(e.Text);
        }

        private static bool IsPositiveInteger(string text)
        {
            Regex regex = new Regex("[0-9]+");
            return !regex.IsMatch(text);
        }

        private void ForwardCheckButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BacktrackingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration.BoardSize = Int32.Parse(BoardSizeTextBox.Text);
            }
            catch (Exception exc)
            {
                Configuration.BoardSize = 0;
            }

            var solution = BacktrackingExecutor.FindSolution(Configuration.BoardSize);

            RedrawBoard(solution);
        }
    }
}
