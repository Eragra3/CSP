﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
using Sudoku.Models;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RedrawBoard();
        }

        public void RedrawBoard()
        {
            BoardPanel.Children.Clear();
            BoardPanel.RowDefinitions.Clear();
            BoardPanel.ColumnDefinitions.Clear();

            for (var i = 0; i < Configuration.BoardSize; i++)
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

            for (var i = 0; i < Configuration.BoardSize; i++)
            {
                for (var j = 0; j < Configuration.BoardSize; j++)
                {
                    var tile = new Tile();
                    if (j % 2 == 0) tile.Background = Brushes.NavajoWhite;

                    BoardPanel.Children.Add(tile);
                }
            }
        }
    }
}