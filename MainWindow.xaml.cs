using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    private HarmonicConstituents constituents = new HarmonicConstituents(2017, 07, 22, 1);

    public List<DisplayRow> TableData { get; set; }

    public MainWindow()
    {
      this.TableData = new List<DisplayRow>();
      this.CalculateTides();
      this.DataContext = this;
      InitializeComponent();
    }

    public void CalculateTides()
    {
      this.UpdateForOlympia();

      //this.UpdateForSeattle();

      //AddTidesToGoogleCalendar(DateTime.Now.Date + new TimeSpan(1,0,0,0,0), 24 * 7, 7);
      //AddSlackTidesToGoogleCalendar(DateTime.Now.Date + new TimeSpan(1, 0, 0, 0, 0), 24 * 7);


      this.AddDisplay(DateTime.Now);
      TableData.Add(new DisplayRow());

      List<double> timesToDisplayToday = new List<double>();
      ///timesToDisplayToday.AddRange(this.constituents.FindTimesForHeight(7, HoursSinceEpochStart(DateTime.Now.Date), 24));
      timesToDisplayToday.AddRange(this.constituents.FindTimesForRateOfChange(0, HoursSinceEpochStart(DateTime.Now.Date), 24 * 2));

      timesToDisplayToday = timesToDisplayToday.OrderBy(i => i).ToList();
      
      foreach (double d in timesToDisplayToday)
      {
        this.AddDisplay(this.DateTimeFromHoursSinceEpochStart(d));
      }


      ////TableData.Add(new DisplayRow());

      ////List<double> timesForSevenFootTides = this.constituents.FindTimesForHeight(7, HoursSinceEpochStart(DateTime.Now), 365 * 24);
      ////foreach (double hours in timesForSevenFootTides)
      ////{
      ////  this.AddDisplay(this.DateTimeFromHoursSinceEpochStart(hours));
      ////}
    }

    public void UpdateForOlympia()
    {
      // these are in feet and degrees 
      this.constituents.Constituents[Constants.M2].Amplitude = 4.79;
      this.constituents.Constituents[Constants.M2].Phase = 30.3;

      this.constituents.Constituents[Constants.S2].Amplitude = 1.13;
      this.constituents.Constituents[Constants.S2].Phase = 62.0;

      this.constituents.Constituents[Constants.N2].Amplitude = 0.91;
      this.constituents.Constituents[Constants.N2].Phase = 4.5;

      this.constituents.Constituents[Constants.K1].Amplitude = 2.88;
      this.constituents.Constituents[Constants.K1].Phase = 289.6;

      this.constituents.Constituents[Constants.M4].Amplitude = 0.16;
      this.constituents.Constituents[Constants.M4].Phase = 294.4;

      this.constituents.Constituents[Constants.O1].Amplitude = 1.55;
      this.constituents.Constituents[Constants.O1].Phase = 266.4;

      this.constituents.Constituents[Constants.M6].Amplitude = 0.1;
      this.constituents.Constituents[Constants.M6].Phase = 143.8;

      this.constituents.Constituents[Constants.MK3].Amplitude = 0.1;
      this.constituents.Constituents[Constants.MK3].Phase = 145.8;

      this.constituents.Constituents[Constants.S4].Amplitude = 0.01;
      this.constituents.Constituents[Constants.S4].Phase = 334.8;

      this.constituents.Constituents[Constants.MN4].Amplitude = 0.07;
      this.constituents.Constituents[Constants.MN4].Phase = 265.4;

      this.constituents.Constituents[Constants.NU2].Amplitude = 0.21;
      this.constituents.Constituents[Constants.NU2].Phase = 10.6;

      this.constituents.Constituents[Constants.S6].Amplitude = 0.00;
      this.constituents.Constituents[Constants.S6].Phase = 0.0;

      this.constituents.Constituents[Constants.MU2].Amplitude = 0.08;
      this.constituents.Constituents[Constants.MU2].Phase = 193.0;

      this.constituents.Constituents[Constants._2N2].Amplitude = 0.11;
      this.constituents.Constituents[Constants._2N2].Phase = 345.2;

      this.constituents.Constituents[Constants.OO1].Amplitude = 0.09;
      this.constituents.Constituents[Constants.OO1].Phase = 5.6;

      this.constituents.Constituents[Constants.LAMBDA2].Amplitude = 0.09;
      this.constituents.Constituents[Constants.LAMBDA2].Phase = 46.8;

      this.constituents.Constituents[Constants.S1].Amplitude = 0.06;
      this.constituents.Constituents[Constants.S1].Phase = 63.5;

      this.constituents.Constituents[Constants.M1].Amplitude = 0.06;
      this.constituents.Constituents[Constants.M1].Phase = 309.0;

      this.constituents.Constituents[Constants.J1].Amplitude = 0.17;
      this.constituents.Constituents[Constants.J1].Phase = 331.4;

      this.constituents.Constituents[Constants.MM].Amplitude = 0.00;
      this.constituents.Constituents[Constants.MM].Phase = 00.0;

      this.constituents.Constituents[Constants.SSA].Amplitude = 0.11;
      this.constituents.Constituents[Constants.SSA].Phase = 231.1;

      this.constituents.Constituents[Constants.SA].Amplitude = 0.25;
      this.constituents.Constituents[Constants.SA].Phase = 292.9;

      this.constituents.Constituents[Constants.MSF].Amplitude = 0.0;
      this.constituents.Constituents[Constants.MSF].Phase = 0.0;

      this.constituents.Constituents[Constants.MF].Amplitude = 0.07;
      this.constituents.Constituents[Constants.MF].Phase = 140.5;

      this.constituents.Constituents[Constants.RHO1].Amplitude = 0.08;
      this.constituents.Constituents[Constants.RHO1].Phase = 269.8;

      this.constituents.Constituents[Constants.Q1].Amplitude = 0.23;
      this.constituents.Constituents[Constants.Q1].Phase = 264.7;

      this.constituents.Constituents[Constants.T2].Amplitude = 0.05;
      this.constituents.Constituents[Constants.T2].Phase = 69.9;

      this.constituents.Constituents[Constants.R2].Amplitude = 0.04;
      this.constituents.Constituents[Constants.R2].Phase = 118.3;

      this.constituents.Constituents[Constants._2Q1].Amplitude = 0.03;
      this.constituents.Constituents[Constants._2Q1].Phase = 238.2;

      this.constituents.Constituents[Constants.P1].Amplitude = 0.88;
      this.constituents.Constituents[Constants.P1].Phase = 285.3;

      this.constituents.Constituents[Constants._2SM2].Amplitude = 0.05;
      this.constituents.Constituents[Constants._2SM2].Phase = 273.4;

      this.constituents.Constituents[Constants.M3].Amplitude = 0.02;
      this.constituents.Constituents[Constants.M3].Phase = 254.8;

      this.constituents.Constituents[Constants.L2].Amplitude = 0.22;
      this.constituents.Constituents[Constants.L2].Phase = 64.4;

      this.constituents.Constituents[Constants._2MK3].Amplitude = 0.02;
      this.constituents.Constituents[Constants._2MK3].Phase = 267.3;

      this.constituents.Constituents[Constants.K2].Amplitude = 0.32;
      this.constituents.Constituents[Constants.K2].Phase = 58.8;

      this.constituents.Constituents[Constants.M8].Amplitude = 0.01;
      this.constituents.Constituents[Constants.M8].Phase = 18.8;

      this.constituents.Constituents[Constants.MS4].Amplitude = 0.1;
      this.constituents.Constituents[Constants.MS4].Phase = 313.7;




      // this.constituents.HourlyOffset = -0.25;
      this.constituents.MeanLowerLowWater = 8.333;

    }

    /// <summary>
    /// update with NOAA harmonic constituents for Seattle 9447130
    /// </summary>
    public void UpdateForSeattle()
    {
      // these are in feet and degrees 
      this.constituents.Constituents[Constants.M2].Amplitude = 3.52;
      this.constituents.Constituents[Constants.M2].Phase = 10.6;

      this.constituents.Constituents[Constants.S2].Amplitude = 0.88;
      this.constituents.Constituents[Constants.S2].Phase = 37.0;

      this.constituents.Constituents[Constants.N2].Amplitude = 0.71;
      this.constituents.Constituents[Constants.N2].Phase = 340.8;

      this.constituents.Constituents[Constants.K1].Amplitude = 2.74;
      this.constituents.Constituents[Constants.K1].Phase = 277.0;

      this.constituents.Constituents[Constants.M4].Amplitude = 0.07;
      this.constituents.Constituents[Constants.M4].Phase = 200.2;

      this.constituents.Constituents[Constants.O1].Amplitude = 1.51;
      this.constituents.Constituents[Constants.O1].Phase = 254.6;

      this.constituents.Constituents[Constants.M6].Amplitude = 0.03;
      this.constituents.Constituents[Constants.M6].Phase = 313.6;

      this.constituents.Constituents[Constants.MK3].Amplitude = 0.11;
      this.constituents.Constituents[Constants.MK3].Phase = 78.1;

      this.constituents.Constituents[Constants.S4].Amplitude = 0.00;
      this.constituents.Constituents[Constants.S4].Phase = 0.0;

      this.constituents.Constituents[Constants.MN4].Amplitude = 0.03;
      this.constituents.Constituents[Constants.MN4].Phase = 174.0;

      this.constituents.Constituents[Constants.NU2].Amplitude = 0.15;
      this.constituents.Constituents[Constants.NU2].Phase = 354.9;

      this.constituents.Constituents[Constants.S6].Amplitude = 0.00;
      this.constituents.Constituents[Constants.S6].Phase = 0.0;

      this.constituents.Constituents[Constants.MU2].Amplitude = 0.11;
      this.constituents.Constituents[Constants.MU2].Phase = 236.6;

      this.constituents.Constituents[Constants._2N2].Amplitude = 0.08;
      this.constituents.Constituents[Constants._2N2].Phase = 308.7;

      this.constituents.Constituents[Constants.OO1].Amplitude = 0.1;
      this.constituents.Constituents[Constants.OO1].Phase = 328.9;

      ////this.constituents.Constituents[Constants._2N2].Amplitude = 0.1;
      ////this.constituents.Constituents[Constants._2N2].Phase = 328.9;

      this.constituents.Constituents[Constants.LAMBDA2].Amplitude = 0.07;
      this.constituents.Constituents[Constants.LAMBDA2].Phase = 48.4;

      this.constituents.Constituents[Constants.S1].Amplitude = 0.05;
      this.constituents.Constituents[Constants.S1].Phase = 44.2;

      this.constituents.Constituents[Constants.M1].Amplitude = 0.09;
      this.constituents.Constituents[Constants.M1].Phase = 319.8;

      this.constituents.Constituents[Constants.J1].Amplitude = 0.13;
      this.constituents.Constituents[Constants.J1].Phase = 315.2;

      this.constituents.Constituents[Constants.MM].Amplitude = 0.00;
      this.constituents.Constituents[Constants.MM].Phase = 00.0;

      this.constituents.Constituents[Constants.SSA].Amplitude = 0.11;
      this.constituents.Constituents[Constants.SSA].Phase = 231.1;

      this.constituents.Constituents[Constants.SA].Amplitude = 0.25;
      this.constituents.Constituents[Constants.SA].Phase = 292.9;

      this.constituents.Constituents[Constants.MSF].Amplitude = 0.0;
      this.constituents.Constituents[Constants.MSF].Phase = 0.0;

      this.constituents.Constituents[Constants.MF].Amplitude = 0.07;
      this.constituents.Constituents[Constants.MF].Phase = 140.5;

      this.constituents.Constituents[Constants.RHO1].Amplitude = 0.05;
      this.constituents.Constituents[Constants.RHO1].Phase = 246.6;

      this.constituents.Constituents[Constants.Q1].Amplitude = 0.25;
      this.constituents.Constituents[Constants.Q1].Phase = 249.9;

      this.constituents.Constituents[Constants.T2].Amplitude = 0.05;
      this.constituents.Constituents[Constants.T2].Phase = 37.1;

      this.constituents.Constituents[Constants.R2].Amplitude = 0.01;
      this.constituents.Constituents[Constants.R2].Phase = 38.0;

      this.constituents.Constituents[Constants._2Q1].Amplitude = 0.03;
      this.constituents.Constituents[Constants._2Q1].Phase = 251.5;

      this.constituents.Constituents[Constants.P1].Amplitude = 0.85;
      this.constituents.Constituents[Constants.P1].Phase = 276.6;

      this.constituents.Constituents[Constants._2SM2].Amplitude = 0.03;
      this.constituents.Constituents[Constants._2SM2].Phase = 282.6;

      this.constituents.Constituents[Constants.M3].Amplitude = 0.02;
      this.constituents.Constituents[Constants.M3].Phase = 342.4;

      this.constituents.Constituents[Constants.L2].Amplitude = 0.15;
      this.constituents.Constituents[Constants.L2].Phase = 56.1;

      this.constituents.Constituents[Constants._2MK3].Amplitude = 0.11;
      this.constituents.Constituents[Constants._2MK3].Phase = 46.7;

      this.constituents.Constituents[Constants.K2].Amplitude = 0.26;
      this.constituents.Constituents[Constants.K2].Phase = 37.9;

      this.constituents.Constituents[Constants.M8].Amplitude = 0.0;
      this.constituents.Constituents[Constants.M8].Phase = 0.0;

      this.constituents.Constituents[Constants.MS4].Amplitude = 0.04;
      this.constituents.Constituents[Constants.MS4].Phase = 229.9;


      this.constituents.HourlyOffset = 0;
      this.constituents.MeanLowerLowWater = 6.623; /// 8.333 - 1.41 - 0.35; //7.94?

    }


    private void AddTidesToGoogleCalendar(DateTime startTime, double hoursToUpdate, double height)
    {
      List<Google.Apis.Calendar.v3.Data.Event> calendarEvents = new List<Google.Apis.Calendar.v3.Data.Event>();

      // find all {height} foot tides for today
      List<double> sevenFootTides = this.constituents.FindTimesForHeight(height, this.HoursSinceEpochStart(startTime), hoursToUpdate);
      double previousTideSpeed = -1000;
      DateTime previousSevenFootTime = DateTime.Now;
      // now build google calendar events for each {height} foot tide
      foreach (double sevenFootTide in sevenFootTides)
      {
        Google.Apis.Calendar.v3.Data.Event calendarEvent = new Google.Apis.Calendar.v3.Data.Event();
        DateTime sevenFootTime = this.DateTimeFromHoursSinceEpochStart(sevenFootTide);
        double tideHeight = this.constituents.PredictTideHeight(sevenFootTide);
        double tideSpeed = this.constituents.PredictRateOfChange(sevenFootTide);
        double tideAcceleration = this.constituents.PredictAccelerationOfChange(sevenFootTide);

        calendarEvent.Summary = $"{height.ToString("0.0")}ft+ tide ";

        if (tideSpeed < 0 && previousTideSpeed > 0)
        {
          calendarEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = previousSevenFootTime, TimeZone = "America/Los_Angeles" };
          calendarEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = sevenFootTime, TimeZone = "America/Los_Angeles" };
          calendarEvents.Add(calendarEvent);
        }

        previousTideSpeed = tideSpeed;
        previousSevenFootTime = sevenFootTime;
      }
      // now add all tide events to the google calendar
      this.AddGoogleCalendarEvents(calendarEvents, "fvstjtakaqodoqs3flv76tqg7o@group.calendar.google.com");
    }

    private void AddSlackTidesToGoogleCalendar(DateTime startTime, double hoursToUpdate)
    {
      List<Google.Apis.Calendar.v3.Data.Event> calendarEvents = new List<Google.Apis.Calendar.v3.Data.Event>();

      // find all slack tides for today
      List<double> slackTides = this.constituents.FindTimesForRateOfChange(0, this.HoursSinceEpochStart(startTime), hoursToUpdate);
      // now build google calendar events for each slack tide
      foreach (double slackHoursSinceEpochStart in slackTides)
      {
        Google.Apis.Calendar.v3.Data.Event calendarEvent = new Google.Apis.Calendar.v3.Data.Event();
        DateTime slackTime = this.DateTimeFromHoursSinceEpochStart(slackHoursSinceEpochStart);
        double tideHeight = this.constituents.PredictTideHeight(slackHoursSinceEpochStart);
        double tideSpeed = this.constituents.PredictRateOfChange(slackHoursSinceEpochStart);
        double tideAcceleration = this.constituents.PredictAccelerationOfChange(slackHoursSinceEpochStart);

        calendarEvent.Summary = "High Tide " + tideHeight.ToString("0.0") + " ft";
        if (tideAcceleration > 0)
        {
          calendarEvent.Summary = "Low Tide " + tideHeight.ToString("0.0") + " ft";
        }

        calendarEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = slackTime, TimeZone = "America/Los_Angeles" };
        calendarEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = slackTime + new TimeSpan(0, 5, 0), TimeZone = "America/Los_Angeles" };
        calendarEvents.Add(calendarEvent);
      }

      // now add all tide events to the google calendar
      this.AddGoogleCalendarEvents(calendarEvents, "fvstjtakaqodoqs3flv76tqg7o@group.calendar.google.com");
    }

    private void AddGoogleCalendarEvents(List<Google.Apis.Calendar.v3.Data.Event> calendarEvents, string calendarID = "primary")
    {
      // If modifying these scopes, delete your previously saved credentials
      // at ~/.credentials/calendar-dotnet-quickstart.json
      // static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
      string[] scopes = { Google.Apis.Calendar.v3.CalendarService.Scope.Calendar };

      ////https://www.googleapis.com/auth/calendar
      string ApplicationName = "Google Calendar API .NET Quickstart";

      Google.Apis.Auth.OAuth2.UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        credPath = System.IO.Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

        credential = Google.Apis.Auth.OAuth2.GoogleWebAuthorizationBroker.AuthorizeAsync(
            Google.Apis.Auth.OAuth2.GoogleClientSecrets.Load(stream).Secrets,
            scopes,
            "user",
            System.Threading.CancellationToken.None,
            new Google.Apis.Util.Store.FileDataStore(credPath, true)).Result;
        ////Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Google Calendar API service.
      var service = new Google.Apis.Calendar.v3.CalendarService(new Google.Apis.Services.BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      foreach (Google.Apis.Calendar.v3.Data.Event calendarEvent in calendarEvents)
      {
        Google.Apis.Calendar.v3.EventsResource.InsertRequest insertRequest = service.Events.Insert(calendarEvent, calendarID);
        insertRequest.Execute();
      }
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
        TideHeight = this.constituents.PredictTideHeight(hoursSinceEpochStart).ToString("0.00"),
        TideSpeed = this.constituents.PredictRateOfChange(hoursSinceEpochStart).ToString("0.00"),
        TideAcceleration = this.constituents.PredictAccelerationOfChange(hoursSinceEpochStart).ToString("0.00")
      });
    }

  }
}
