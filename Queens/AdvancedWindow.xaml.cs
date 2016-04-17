using Queens.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using System.Windows.Shapes;
using Queens.Logic;

namespace Queens
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

            DataContext = new
            {
                ExperimentResults = _experimentResults
            };
        }

        private async void StartExperimentBacktracking()
        {
            var config = ReadValues(ExecutorsEnum.Backtracking);
            var iterator = CSPExecutor.RunExperiment(config);

            foreach (var run in iterator)
            {
                _experimentResults.Add(run);
                await Task.Delay(100);
            }
        }
        private async void StartExperimentForwardChecking()
        {
            var config = ReadValues(ExecutorsEnum.ForwardChecking);
            var iterator = CSPExecutor.RunExperiment(config);

            foreach (var run in iterator)
            {
                _experimentResults.Add(run);
                await Task.Delay(100);
            }
        }


        private ConfigurationBatchFile ReadValues(ExecutorsEnum usedExecutor)
        {
            int minN;
            int maxN;
            ValuePickingHeuristicsEnum valuePickingMethod;
            VariablePickingHeuristicsEnum variablePickingMethod;
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
                valuePickingMethod = (ValuePickingHeuristicsEnum)
                    Enum.Parse(typeof(ValuePickingHeuristicsEnum), RowPickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                valuePickingMethod = ValuePickingHeuristicsEnum.Increment;
            }
            try
            {
                variablePickingMethod = (VariablePickingHeuristicsEnum)
                    Enum.Parse(typeof(VariablePickingHeuristicsEnum), QueenPickingMethodComboBox.Text);
            }
            catch (Exception)
            {
                variablePickingMethod = VariablePickingHeuristicsEnum.Random;
            }

            return new ConfigurationBatchFile(
                minN,
                maxN,
                valuePickingMethod,
                variablePickingMethod,
                usedExecutor);
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
