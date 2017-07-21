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
    private HarmonicConstituents constituents = new HarmonicConstituents(1983, 01, 01, 360);

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
      //this.UpdateForOlympia();

      this.UpdateForSeattle();

      //AddTidesToGoogleCalendar(DateTime.Now.Date + new TimeSpan(1,0,0,0,0), 24 * 7, 7);
      //AddSlackTidesToGoogleCalendar(DateTime.Now.Date + new TimeSpan(1, 0, 0, 0, 0), 24 * 7);


      this.AddDisplay(DateTime.Now);
      TableData.Add(new DisplayRow());

      List<double> timesToDisplayToday = this.constituents.FindTimesForHeight(7, HoursSinceEpochStart(DateTime.Now.Date), 24);
      timesToDisplayToday.AddRange(this.constituents.FindTimesForRateOfChange(0, HoursSinceEpochStart(DateTime.Now.Date), 24));

      timesToDisplayToday = timesToDisplayToday.OrderBy(i => i).ToList();
      
      foreach (double d in timesToDisplayToday)
      {
        this.AddDisplay(this.DateTimeFromHoursSinceEpochStart(d));
      }


      TableData.Add(new DisplayRow());

      List<double> timesForSevenFootTides = this.constituents.FindTimesForHeight(7, HoursSinceEpochStart(DateTime.Now), 365 * 24);
      foreach (double hours in timesForSevenFootTides)
      {
        this.AddDisplay(this.DateTimeFromHoursSinceEpochStart(hours));
      }
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

      this.constituents.Constituents[Constants.SA].Amplitude = 0.25;
      this.constituents.Constituents[Constants.SA].Phase = 292.9;

      this.constituents.Constituents[Constants.Q1].Amplitude = 0.25;
      this.constituents.Constituents[Constants.Q1].Phase = 249.9;

      this.constituents.Constituents[Constants.P1].Amplitude = 0.85;
      this.constituents.Constituents[Constants.P1].Phase = 276.6;



      this.constituents.HourlyOffset = 0;
      this.constituents.MeanLowerLowWater = 8.333 - 1.41; //7.94

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
        TideHeight = this.constituents.PredictTideHeight(hoursSinceEpochStart).ToString("0.0"),
        TideSpeed = this.constituents.PredictRateOfChange(hoursSinceEpochStart).ToString("0.0"),
        TideAcceleration = this.constituents.PredictAccelerationOfChange(hoursSinceEpochStart).ToString("0.0")
      });
    }

  }
}
