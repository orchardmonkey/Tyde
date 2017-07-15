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
      List<Google.Apis.Calendar.v3.Data.Event> calendarEvents = new List<Google.Apis.Calendar.v3.Data.Event>();

      // find all slack tides for today
      List<double> slackTides = this.constituents.FindTimesForRateOfChange(0, this.HoursSinceEpochStart(DateTime.Now.Date), 24);
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

        //newEvent.Summary = "7ft+ tide";
        calendarEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = slackTime, TimeZone = "America/Los_Angeles" };
        calendarEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime() { DateTime = slackTime + new TimeSpan(0, 5, 0), TimeZone = "America/Los_Angeles" };
        calendarEvents.Add(calendarEvent);
      }
      // now add all slack tides to google calendar
      this.AddGoogleCalendarEvents(calendarEvents, "primary");



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

    public void AddGoogleCalendarEvents(List<Google.Apis.Calendar.v3.Data.Event> calendarEvents, string calendarID = "primary")
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
