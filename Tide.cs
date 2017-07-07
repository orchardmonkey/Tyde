using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  public class Tide
  {
    HarmonicConstituent[] constituents;

    ////List<HarmonicConstituent> constituents = new List<HarmonicConstituent>();
    DateTime epochStartGMT = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc) ;
//    DateTime epochStartGMT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc);

    public Tide()
    {
      //this.constituents = HarmonicConstituent.GetInitializedConstituents();
      ////epochStartGMT = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      //epoch parms passed into Equilibrium tide need to be in GMT.  Probably Jan 1, 0 hour.
      EquilibriumTide equilibriumTide = new EquilibriumTide(epochStartGMT.Year, epochStartGMT.Month, epochStartGMT.Day, 30);
      this.constituents = equilibriumTide.Constituents;


      this.UpdateConstituentsForOlympia();

    }

    // update phases and amplitudes of the tidal constituents for Olympia.  Amplitudes are in meters.  Phases are in degrees.
    public void UpdateConstituentsForOlympia()
    {
      ////this.constituents[Constants.M2].Amplitude = 1.464;
      ////this.constituents[Constants.M2].Phase = 29.9;

      ////this.constituents[Constants.K1].Amplitude = 0.849;
      ////this.constituents[Constants.K1].Phase = 288.7;

      ////this.constituents[Constants.O1].Amplitude = 0.463;
      ////this.constituents[Constants.O1].Phase = 265.1;

      ////this.constituents[Constants.S2].Amplitude = 0.348;
      ////this.constituents[Constants.S2].Phase = 062.4;

      ////this.constituents[Constants.N2].Amplitude = 0.281;
      ////this.constituents[Constants.N2].Phase = 004.3;

      ////this.constituents[Constants.P1].Amplitude = 0.248;
      ////this.constituents[Constants.P1].Phase = 287.6;

      ////this.constituents[Constants.M4].Amplitude = 0.055;
      ////this.constituents[Constants.M4].Phase = 291.0;

      ////this.constituents[Constants.M6].Amplitude = 0.032;
      ////this.constituents[Constants.M6].Phase = 142.1;

      // these are in feet and degrees 
      this.constituents[Constants.M2].Amplitude = 4.79;
      this.constituents[Constants.M2].Phase = 30.3;

      this.constituents[Constants.K1].Amplitude = 2.88;
      this.constituents[Constants.K1].Phase = 289.6;

      this.constituents[Constants.O1].Amplitude = 1.55;
      this.constituents[Constants.O1].Phase = 266.4;

      this.constituents[Constants.S2].Amplitude = 1.13;
      this.constituents[Constants.S2].Phase = 062;

      this.constituents[Constants.N2].Amplitude = 0.91;
      this.constituents[Constants.N2].Phase = 004.5;

      this.constituents[Constants.P1].Amplitude = 0.88;
      this.constituents[Constants.P1].Phase = 285.3;

      this.constituents[Constants.M4].Amplitude = 0.16;
      this.constituents[Constants.M4].Phase = 294.4;

      this.constituents[Constants.M6].Amplitude = 0.1;
      this.constituents[Constants.M6].Phase = 143.8;



      // equilibrium phase is calculated for the epoch start, and is the phase of the constituent for the meridian 0 degrees longitude, GMT, assuming perfect tidal bulge, no continents or shallow shelves.
      // so, the equilbrium phase for the sun at the meridian, 0:00 am GMT is always 0, because the sun is at nadir.
      // https://en.wikipedia.org/wiki/Arthur_Thomas_Doodson
      // add in equilibrium phases, until we finish code to calculate them.
      ////this.constituents[Constants.M2].EquilibriumPhase = 270.21;
      ////this.constituents[Constants.K1].EquilibriumPhase = 017.70;
      ////this.constituents[Constants.O1].EquilibriumPhase = 248.94;
      ////this.constituents[Constants.S2].EquilibriumPhase = 000.00;
      ////this.constituents[Constants.N2].EquilibriumPhase = 016.12;
      ////this.constituents[Constants.P1].EquilibriumPhase = 349.19;
      ////this.constituents[Constants.M4].EquilibriumPhase = 180.42;
      ////this.constituents[Constants.M6].EquilibriumPhase = 090.63;


    }

    /// <summary>
    /// use Newton's method to solve for zero.
    /// basically, our next x value is current x - f(x)/f1(x)
    /// where f1(x) is the derivative of f(x) with respect to x
    /// in this case, f1(x) is PredictRateOfChange
    /// and f(x) is PredictTideHeight
    /// </summary>
    /// <param name="height">height of tide we wish to calculate occurrence time for</param>
    /// <returns></returns>
    public DateTime? FindTimeForHeight(double targetHeight, DateTime startTime)
    {
      DateTime targetTime = startTime;

      for (int i = 0; i < 5; i++)
      {
        double startingHeight = PredictTideHeight(targetTime);
        double startingSlope = PredictRateOfChange(targetTime);
        double hours = HoursSinceEpochStart(targetTime);
        double nexthours = hours - (startingHeight - targetHeight) / startingSlope;
        targetTime = DateTimeFromHoursSinceEpochStart(nexthours);
      }

      double endHeight = PredictTideHeight(targetTime);
      if (endHeight - targetHeight > .05 || endHeight - targetHeight < -.05)
      {
        return null;
      }

      // sometimes if our start time is too close to slack tide, then the slope is almost zero.
      // so when we divide by the slope to get our next x, we jump many days - returning a value that is too far off.
      if (targetTime - startTime > new TimeSpan(3,0,0) || startTime - targetTime > new TimeSpan(3,0,0) )
      {
        return null;
      }

      return targetTime;
    }

    /// <summary>
    /// Does the opposite of HoursSinceEpochStart
    /// </summary>
    /// <param name="hours"></param>
    /// <returns></returns>
    private DateTime DateTimeFromHoursSinceEpochStart(double hours)
    {
      DateTime dateTime = epochStartGMT + TimeSpan.FromHours(hours);
      dateTime = dateTime.ToLocalTime();
      return dateTime;
    }

    private double HoursSinceEpochStart(DateTime timeOfCalculation)
    {
      timeOfCalculation = timeOfCalculation.ToUniversalTime(); //calculate GMT at same instant.
      double hoursSinceEpochStart = (timeOfCalculation - epochStartGMT).TotalHours;
      return hoursSinceEpochStart;
    }


    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="dateTime">local time that we wish to calculate Olympia tides for</param>
    /// <returns>height of tide</returns>
    public double PredictTideHeight(DateTime timeOfCalculation)
    {
      // for some reason, we are about 1/2 hour early in predictions for Budd Inlet, etc.
      // i.e. for a given time we are displaying the tide as it will be ~30 minutes in the future.
      timeOfCalculation = timeOfCalculation - new TimeSpan(0, 15, 0);

      return HarmonicConstituent.PredictTideHeight(HoursSinceEpochStart(timeOfCalculation), this.constituents, 8.333);
    }

    public double PredictRateOfChange(DateTime timeOfCalculation)
    {
      // for some reason, we are about 1/2 hour early in predictions for Budd Inlet, etc.
      // i.e. for a given time we are displaying the tide as it will be ~30 minutes in the future.
      timeOfCalculation = timeOfCalculation - new TimeSpan(0, 15, 0);

      return HarmonicConstituent.PredictRateOfChange(HoursSinceEpochStart(timeOfCalculation), this.constituents);
    }


  }
}
