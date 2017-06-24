using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  public class Tide
  {
    List<HarmonicConstituent> constituents = new List<HarmonicConstituent>();
    DateTime epochStart = DateTime.Now;

    public Tide()
    {
      InitializeConstituentsForOlympia();
    }

    public void InitializeConstituentsForOlympia()
    {
      // Phase is in degrees.  They vary based upon location, but are constant for each location, because they are offset from the equilibrium phase.
      // Amplitude is in meters here.  They vary based upon location, but are constant for each location, except there is a modification for the moon's 19 year nodal precession.  Usually an amplitude modifier is calculated for each constituent, and is treated as a constant for the entire year.
      // Constituent speeds are in degrees per hour, and are calculated from the 6 doodson numbers, but are hardcoded here.  they are the same for any location
      // equilibrium phase is calculated for the epoch start, and is the phase of the constituent for the meridian 0 degrees longitude, GMT, assuming perfect tidal bulge, no continents or shallow shelves.
      // so, the equilbrium phase for the sun at the meridian, 0:00 am GMT is always 0, because the sun is at nadir.
      // https://en.wikipedia.org/wiki/Arthur_Thomas_Doodson
      constituents.Add(new HarmonicConstituent() { ID = "M2", Speed = 28.9841042, Amplitude = 1.464, Phase = 029.9, EquilibriumPhase = 270.21, Description = "Principal lunar, semidiurnal" });
      constituents.Add(new HarmonicConstituent() { ID = "K1", Speed = 15.0410686, Amplitude = 0.849, Phase = 288.7, EquilibriumPhase = 017.70, Description = "Sun-Moon angle, diurnal" });
      constituents.Add(new HarmonicConstituent() { ID = "O1", Speed = 13.9430356, Amplitude = 0.463, Phase = 265.1, EquilibriumPhase = 248.94, Description = "Principal lunar declination" });
      constituents.Add(new HarmonicConstituent() { ID = "S2", Speed = 30.0000000, Amplitude = 0.348, Phase = 062.4, EquilibriumPhase = 000.00, Description = "Principal solar, semidiurnal" });
      constituents.Add(new HarmonicConstituent() { ID = "N2", Speed = 28.4397295, Amplitude = 0.281, Phase = 004.3, EquilibriumPhase = 016.12, Description = "Principal lunar elliptic, semidiurnal" });
      constituents.Add(new HarmonicConstituent() { ID = "P1", Speed = 14.9589314, Amplitude = 0.248, Phase = 287.6, EquilibriumPhase = 349.19, Description = "Principal solar declination" });
      constituents.Add(new HarmonicConstituent() { ID = "M4", Speed = 57.9682084, Amplitude = 0.055, Phase = 291.0, EquilibriumPhase = 180.42, Description = "Shallow water" });
      constituents.Add(new HarmonicConstituent() { ID = "M6", Speed = 86.9523127, Amplitude = 0.032, Phase = 142.1, EquilibriumPhase = 090.63, Description = "Shallow water" });

      epochStart = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
    }

    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="hoursSinceEpochStart">hours since the start of the epoch  that our constituent Amplitudes and Phases were calculated for</param>
    /// <returns>height of tide in feet</returns>
    private double PredictTideHeight(double hoursSinceEpochStart)
    {
      double tideInMeters =  constituents.Sum(c => c.Height(hoursSinceEpochStart));
      return tideInMeters * 3.28 + 8.5; // convert meters to feet and add mean water level of ~9 (guess)


    }

    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="dateTime">local time that we wish to calculate Olympia tides for</param>
    /// <returns>height of tide</returns>
    public double PredictTideHeight(DateTime timeOfCalculation)
    {
      double hoursSinceEpochStart = (timeOfCalculation - epochStart).TotalHours;
      return PredictTideHeight(hoursSinceEpochStart);
    }


  }
}
