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
    DateTime epochStartGMT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc) ;

    public Tide()
    {
      //this.constituents = HarmonicConstituent.GetInitializedConstituents();
      epochStartGMT = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      //epoch parms passed into Equilibrium tide need to be in GMT.  Probably Jan 1, 0 hour.
      EquilibriumTide equilibriumTide = new EquilibriumTide(epochStartGMT.Year, epochStartGMT.Month, epochStartGMT.Day, 30);
      this.constituents = equilibriumTide.Constituents;


      this.UpdateConstituentsForOlympia();

    }

    // update phases and amplitudes of the tidal constituents for Olympia.  Amplitudes are in meters.  Phases are in degrees.
    public void UpdateConstituentsForOlympia()
    {
      this.constituents[Constants.M2].Amplitude = 1.464;
      this.constituents[Constants.M2].Phase = 29.9;

      this.constituents[Constants.K1].Amplitude = 0.849;
      this.constituents[Constants.K1].Phase = 288.7;

      this.constituents[Constants.O1].Amplitude = 0.463;
      this.constituents[Constants.O1].Phase = 265.1;

      this.constituents[Constants.S2].Amplitude = 0.348;
      this.constituents[Constants.S2].Phase = 062.4;

      this.constituents[Constants.N2].Amplitude = 0.281;
      this.constituents[Constants.N2].Phase = 004.3;

      this.constituents[Constants.P1].Amplitude = 0.248;
      this.constituents[Constants.P1].Phase = 287.6;

      this.constituents[Constants.M4].Amplitude = 0.055;
      this.constituents[Constants.M4].Phase = 291.0;

      this.constituents[Constants.M6].Amplitude = 0.032;
      this.constituents[Constants.M6].Phase = 142.1;


      // equilibrium phase is calculated for the epoch start, and is the phase of the constituent for the meridian 0 degrees longitude, GMT, assuming perfect tidal bulge, no continents or shallow shelves.
      // so, the equilbrium phase for the sun at the meridian, 0:00 am GMT is always 0, because the sun is at nadir.
      // https://en.wikipedia.org/wiki/Arthur_Thomas_Doodson
      // add in equilibrium phases, until we finish code to calculate them.
      this.constituents[Constants.M2].EquilibriumPhase = 270.21;
      this.constituents[Constants.K1].EquilibriumPhase = 017.70;
      this.constituents[Constants.O1].EquilibriumPhase = 248.94;
      this.constituents[Constants.S2].EquilibriumPhase = 000.00;
      this.constituents[Constants.N2].EquilibriumPhase = 016.12;
      this.constituents[Constants.P1].EquilibriumPhase = 349.19;
      this.constituents[Constants.M4].EquilibriumPhase = 180.42;
      this.constituents[Constants.M6].EquilibriumPhase = 090.63;

      
    }

    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="hoursSinceEpochStart">hours since the start of the epoch  that our constituent Amplitudes and Phases were calculated for</param>
    /// <returns>height of tide in feet</returns>
    private double PredictTideHeight(double hoursSinceEpochStart)
    {
      double tideInMeters =  constituents.Sum(c => c.Height(hoursSinceEpochStart));
      return tideInMeters * 3.28 + 8.1; // convert meters to feet and add mean water level of ~9 (guess)


    }

    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="dateTime">local time that we wish to calculate Olympia tides for</param>
    /// <returns>height of tide</returns>
    public double PredictTideHeight(DateTime timeOfCalculation)
    {
      timeOfCalculation = timeOfCalculation.ToUniversalTime();
      double hoursSinceEpochStart = (timeOfCalculation - epochStartGMT).TotalHours;
      return PredictTideHeight(hoursSinceEpochStart);
    }


  }
}
