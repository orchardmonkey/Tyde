using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Runtime.CompilerServices;
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
    public List<DisplayRow> TableData { get; set; }

    public MainWindow()
    {
      this.TableData = new List<DisplayRow>();

      Tide tideCalculator = new Tyde.Tide();
      this.AddDisplayTime(tideCalculator, DateTime.Now);

      TableData.Add(new DisplayRow());

      DateTime startOfToday = DateTime.Now.Date; // + new TimeSpan(365,0,0,0);
      
      for (int hour = 0; hour <= 24; hour++)
      {
        DateTime predictionTime = startOfToday + new TimeSpan(hour, 0, 0);
        this.AddDisplayTime(tideCalculator, predictionTime);
      }

      TableData.Add(new DisplayRow());

      // find 7' tides, for every 2 hour increment.  So try 12x per day  
      int days = 10;
      HashSet<DateTime> sevenTimes = new HashSet<DateTime>();

      
      for (int i = 0; i < days * 365; i++)
      {
        DateTime? sevenTime = tideCalculator.FindTimeForHeight(7, startOfToday + new TimeSpan(i * 2, 0, 0));
        if (sevenTime.HasValue)
        {
          // chop off seconds so the hashset will throw away duplicates
          sevenTimes.Add(new DateTime(sevenTime.Value.Year, sevenTime.Value.Month, sevenTime.Value.Day, sevenTime.Value.Hour, sevenTime.Value.Minute, 0));
        }
      }

      // sort the 7' tides by date and time
      List<DateTime> listSeven = sevenTimes.OrderBy(i => i).ToList();

      foreach (DateTime sevenTime in listSeven)
      {
        this.AddDisplayTime(tideCalculator, sevenTime);
      }


      // now pair up consecutive rising and falling tides.  These will be the beginning and end of our good kayaking times.



      ////this.AddDisplayTime(tideCalculator, tideCalculator.FindTimeForHeight(7, startOfToday ));
      ////this.AddDisplayTime(tideCalculator, tideCalculator.FindTimeForHeight(7, startOfToday + new TimeSpan(6, 0, 0)));
      ////this.AddDisplayTime(tideCalculator, tideCalculator.FindTimeForHeight(7, startOfToday + new TimeSpan(12, 0, 0)));
      ////this.AddDisplayTime(tideCalculator, tideCalculator.FindTimeForHeight(7, startOfToday + new TimeSpan(18, 0, 0)));


      this.DataContext = this;

      InitializeComponent();

    }

    public void AddDisplayTime(Tide tideCalculator, DateTime displayTime)
    {
      TableData.Add(new DisplayRow()
      {
        Time = displayTime.ToShortDateString() + " " + displayTime.ToShortTimeString(),
        TideHeight = tideCalculator.PredictTideHeight(displayTime).ToString("0.0"),
        TideSpeed = tideCalculator.PredictRateOfChange(displayTime).ToString("0.0")
      });

    }

  }
}
