using Queens.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Queens
{
    /// <summary>
    /// Interaction logic for AdvancedWindow.xaml
    /// </summary>
    public partial class AdvancedWindow : Window
    {
        public IList<ExperimentResultStatistics> ExperimentResults { get; set; }

        public AdvancedWindow()
        {
            InitializeComponent();

            ExperimentResults = new List<ExperimentResultStatistics>(100);

            DataContext = new
            {
                ExperimentResults = ExperimentResults
            };
        }

        public void StartExperimentBacktracking()
        {

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
                Configuration.RenderBoard = RenderBoardCheckBox.IsChecked ?? false;
            }
            catch (Exception)
            {
                Configuration.RenderBoard = false;
            }
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
    }
}
