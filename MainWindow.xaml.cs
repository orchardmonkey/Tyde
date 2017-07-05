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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tyde
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Tide tideCalculator = new Tyde.Tide();
      DateTime startOfToday = DateTime.Now.Date;
      double tideNow = tideCalculator.PredictTideHeight(DateTime.Now);
      double rateNow = tideCalculator.PredictRateOfChange(DateTime.Now);
      double tide = tideCalculator.PredictTideHeight(startOfToday);
      double tide1 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(1, 0, 0));
      double tide2 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(2, 0, 0));
      double tide3 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(3, 0, 0));
      double tide4 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(4, 0, 0));
      double tide5 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(5, 0, 0));
      double tide6 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(6, 0, 0));
      double tide7 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(7, 0, 0));
      double tide8 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(8, 0, 0));
      double tide9 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(9, 0, 0));
      double tide10 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(10, 0, 0));
      double tide11 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(11, 0, 0));
      double tide12 = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(12, 0, 0));
      double tide1p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(13, 0, 0));
      double tide2p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(14, 0, 0));
      double tide3p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(15, 0, 0));
      double tide4p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(16, 0, 0));
      double tide5p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(17, 0, 0));
      double tide6p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(18, 0, 0));
      double tide7p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(19, 0, 0));
      double tide8p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(20, 0, 0));
      double tide9p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(21, 0, 0));
      double tide10p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(22, 0, 0));
      double tide11p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(23, 0, 0));
      double tide12p = tideCalculator.PredictTideHeight(startOfToday + new TimeSpan(24, 0, 0));
    }
  }
}
