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

            ExperimentResults = new List<ExperimentResultStatistics>
            {
                new ExperimentResultStatistics
                {
                    N = 8,
                    NumberOfBacktracks = 62
                }
            };

            DataContext = new { ExperimentResults = ExperimentResults };
        }
    }
}
