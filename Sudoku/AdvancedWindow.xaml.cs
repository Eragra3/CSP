using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sudoku.Logic;
using Sudoku.Models;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for AdvancedWindow.xaml
    /// </summary>
    public partial class AdvancedWindow : Window
    {
        private ObservableCollection<SingleRunResultStatistics> _experimentResults { get; set; }

        //private IList<ExperimentResultStatistics>

        public AdvancedWindow()
        {
            InitializeComponent();

            _experimentResults = new ObservableCollection<SingleRunResultStatistics>();

            MinNTextBox.Text = "0";
            MaxNTextBox.Text = "0";
            BoardSizeTextBox.Text = "0";

            DataContext = new
            {
                ExperimentResults = _experimentResults
            };
        }

        private async void StartExperimentBacktracking()
        {
            var config = ReadValues(Configuration.ExecutorsEnum.Backtracking);
            var randomSolution = Configuration.GetRandom3x3Sudoku;

            var iterator = CSPExecutor.RunExperimentRepeat(config, 5, randomSolution);

            foreach (var run in iterator)
            {
                _experimentResults.Add(run);
                await Task.Delay(1);
            }
        }
        private async void StartExperimentForwardChecking()
        {
            var config = ReadValues(Configuration.ExecutorsEnum.ForwardChecking);
            var randomSolution = Configuration.GetRandom3x3Sudoku;

            var iterator = CSPExecutor.RunExperimentRepeat(config, 5, randomSolution);

            foreach (var run in iterator)
            {
                _experimentResults.Add(run);
                await Task.Delay(1);
            }
        }


        private ConfigurationBatchFile ReadValues(Configuration.ExecutorsEnum usedExecutor)
        {
            int minN;
            int maxN;
            Configuration.ValuePickingHeuristicsEnum valuePickingMethod;
            Configuration.VariablePickingHeuristicsEnum variablePickingMethod;
            int boardSize;
            try
            {
                minN = int.Parse(MinNTextBox.Text);
            }
            catch (Exception)
            {
                minN = 0;
            }
            try
            {
                maxN = int.Parse(MaxNTextBox.Text);
            }
            catch (Exception)
            {
                maxN = 0;
            }
            try
            {
                valuePickingMethod = (Configuration.ValuePickingHeuristicsEnum)
                    Enum.Parse(typeof(Configuration.ValuePickingHeuristicsEnum), RowPickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                valuePickingMethod = Configuration.ValuePickingHeuristicsEnum.Increment;
            }
            try
            {
                variablePickingMethod = (Configuration.VariablePickingHeuristicsEnum)
                    Enum.Parse(typeof(Configuration.VariablePickingHeuristicsEnum), QueenPickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                variablePickingMethod = Configuration.VariablePickingHeuristicsEnum.Random;
            }
            try
            {
                boardSize = int.Parse(BoardSizeTextBox.Text);
            }
            catch (Exception)
            {
                boardSize = 9;
            }

            return new ConfigurationBatchFile(
                minN,
                maxN,
                valuePickingMethod,
                variablePickingMethod,
                usedExecutor,
                boardSize);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsPositiveInteger(e.Text);
        }

        private static bool IsPositiveInteger(string text)
        {
            var regex = new Regex("[0-9]+");
            return !regex.IsMatch(text);
        }

        private void StartBacktrackingExperimentButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartExperimentBacktracking();
        }

        private void StartForwardCheckingExperimentButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartExperimentForwardChecking();
        }
    }
}
