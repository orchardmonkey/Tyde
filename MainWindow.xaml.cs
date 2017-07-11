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
    private HarmonicConstituents constituents = new HarmonicConstituents(2017, 01, 01, 360);

    public List<DisplayRow> TableData { get; set; }

    public MainWindow()
    {
      this.TableData = new List<DisplayRow>();
      this.UpdateForOlympia();
      this.CalculateTides();
      this.DataContext = this;
      InitializeComponent();
    }

    public void CalculateTides()
    {
      this.AddDisplay(DateTime.Now);
      TableData.Add(new DisplayRow());

      DateTime startOfToday = DateTime.Now.Date; // + new TimeSpan(365,0,0,0);

      for (int hour = 0; hour <= 24; hour++)
      {
        DateTime predictionTime = startOfToday + new TimeSpan(hour, 0, 0);
        this.AddDisplay(predictionTime);
      }

      TableData.Add(new DisplayRow());

      // find 7' tides, for every 2 hour increment.  So try 12x per day  
      int days = 10;
      HashSet<DateTime> sevenTimes = new HashSet<DateTime>();


      for (int i = 0; i < days * 365; i++)
      {
        double? sevenTime = this.constituents.FindTimeForHeight(7, HoursSinceEpochStart(startOfToday + new TimeSpan(i * 2, 0, 0)));
        //double? sevenTime = this.constituents.FindTimeForRateOfChange(0, HoursSinceEpochStart(startOfToday + new TimeSpan(i * 2, 0, 0)));
        if (sevenTime.HasValue)
        {
          // chop off seconds so the hashset will throw away duplicates
          DateTime dateTime = DateTimeFromHoursSinceEpochStart(sevenTime.Value);
          sevenTimes.Add(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0));
        }
      }

      // sort the 7' tides by date and time
      List<DateTime> listSeven = sevenTimes.OrderBy(i => i).ToList();

      foreach (DateTime sevenTime in listSeven)
      {
        this.AddDisplay(sevenTime);
      }

    }

    public List<double> FindTimesForHeight(double height, double hoursSinceEpochStart, double hoursToSearch)
    {
      HashSet<double> foundHours = new HashSet<double>();

      // search every 2 hours
      for (double searchHour = hoursSinceEpochStart; searchHour < hoursSinceEpochStart + hoursToSearch; searchHour += 2)
      {
        double? foundHour = this.constituents.FindTimeForHeight(height, searchHour);
        //double? sevenTime = this.constituents.FindTimeForRateOfChange(0, HoursSinceEpochStart(startOfToday + new TimeSpan(i * 2, 0, 0)));
        if (foundHour.HasValue)
        {
          // truncate to two decimal places to make it easier to drop duplicates.
          // truncating to two decimal places is approimately truncating to the nearest minute.
          foundHour = ((double)((int)(foundHour.Value * 100))) / 100.0;
          foundHours.Add(foundHour.Value);
        }
      }
      return foundHours.OrderBy(hour => hour).ToList();
    }

    public void UpdateForOlympia()
    {
      // these are in feet and degrees 
      this.constituents.Constituents[Constants.M2].Amplitude = 4.79;
      this.constituents.Constituents[Constants.M2].Phase = 30.3;

      this.constituents.Constituents[Constants.K1].Amplitude = 2.88;
      this.constituents.Constituents[Constants.K1].Phase = 289.6;

      this.constituents.Constituents[Constants.O1].Amplitude = 1.55;
      this.constituents.Constituents[Constants.O1].Phase = 266.4;

      this.constituents.Constituents[Constants.S2].Amplitude = 1.13;
      this.constituents.Constituents[Constants.S2].Phase = 062;

      this.constituents.Constituents[Constants.N2].Amplitude = 0.91;
      this.constituents.Constituents[Constants.N2].Phase = 004.5;

      this.constituents.Constituents[Constants.P1].Amplitude = 0.88;
      this.constituents.Constituents[Constants.P1].Phase = 285.3;

      this.constituents.Constituents[Constants.M4].Amplitude = 0.16;
      this.constituents.Constituents[Constants.M4].Phase = 294.4;

      this.constituents.Constituents[Constants.M6].Amplitude = 0.1;
      this.constituents.Constituents[Constants.M6].Phase = 143.8;

      this.constituents.HourlyOffset = -0.25;
      this.constituents.MeanLowerLowWater = 8.333;

    }

    /// <summary>
    /// Does the opposite of HoursSinceEpochStart
    /// </summary>
    /// <param name="hours"></param>
    /// <returns></returns>
    private DateTime DateTimeFromHoursSinceEpochStart(double hours)
    {
      DateTime epochStartGMT = new DateTime(this.constituents.EpochStartGMTYear, this.constituents.EpochStartGMTMonth, this.constituents.EpochStartGMTDay, 0, 0, 0, DateTimeKind.Utc);
      DateTime dateTime = epochStartGMT + TimeSpan.FromHours(hours);
      dateTime = dateTime.ToLocalTime();
      return dateTime;
    }

    private double HoursSinceEpochStart(DateTime timeOfCalculation)
    {
      DateTime epochStartGMT = new DateTime(this.constituents.EpochStartGMTYear, this.constituents.EpochStartGMTMonth, this.constituents.EpochStartGMTDay, 0, 0, 0, DateTimeKind.Utc);
      timeOfCalculation = timeOfCalculation.ToUniversalTime(); //calculate GMT at same instant.
      double hoursSinceEpochStart = (timeOfCalculation - epochStartGMT).TotalHours;
      return hoursSinceEpochStart;
    }

    private void AddDisplay(DateTime displayTime)
    {
      double hoursSinceEpochStart = HoursSinceEpochStart(displayTime);
      TableData.Add(new DisplayRow()
      {
        Time = displayTime.ToShortDateString() + " " + displayTime.ToShortTimeString(),
        TideHeight = this.constituents.PredictTideHeight(hoursSinceEpochStart).ToString("0.0"),
        TideSpeed = this.constituents.PredictRateOfChange(hoursSinceEpochStart).ToString("0.0"),
        TideAcceleration = this.constituents.PredictAccelerationOfChange(hoursSinceEpochStart).ToString("0.0")
      });
    }

  }
}
