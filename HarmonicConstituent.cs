using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  /// <summary>
  /// A class to store information for each harmonic constituent, to be used for harmonic prediction of tides.
  /// The prediction is performed as:
  /// TidalHeight = Sum of Height for all HarmonicConstituents.
  /// </summary>
  public class HarmonicConstituent
  {
    /// <summary>
    /// the id of the constitutent, e.g. M2, O1, etc.
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// the description of the constituent
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// also called lag.  This value is in degrees, from 0 to 360
    /// the phase is different, depending upon the tidal locale we are predicting for.
    /// the phase is an offset from the theroretical equilibrium phase.
    /// </summary>
    public double Phase { get; set; }

    /// <summary>
    /// The phase for a theoretical equilibrium tide that would exist if there were no continents, just oceans.  
    /// The equilibrium phase for each constituent calculated for the start of the epoch.
    /// </summary>
    public double EquilibriumPhase { get; set; }


    /// <summary>
    /// The node factor is used to modify the amplitude for a given tide locale.  The amplitude is modified because
    /// the moon's node (where the moon's orbit intersects the ecliptic) goes through an approximately 19 year cycle, 
    /// the node factor is calculated for the middle of the epoch we are predicting tides for.
    /// </summary>
    public double NodeFactor { get; set; }


    /// <summary>
    /// could be meters or feet, depending on if the desired tide prediction is to be in meters or feet.
    /// the amplitude is different, depending upon the tidal locale we are predicting for
    /// </summary>
    public double Amplitude { get; set; }

    /// <summary>
    /// the speed of the constituent.  This should be in degrees per hour
    /// These values are always the same for a given constitutent.  For instance:
    /// S2 (principal solar, semi-diurnal) period is 12 hours, so it is always 30 degrees/hour, because 360/12 = 30 degrees/hour
    /// M2 (principal lunar, semi-diurnal) period is 12.42 hours, so it is always 28.98 degrees/hour, because 360/12.42 = 28.98 degrees/hour
    /// </summary>
    public double Speed { get; set; }

    /// <summary>
    /// Calculate the height of this constituent.
    /// </summary>
    /// <param name="h">the number of hours since the start of the epoch that the equilibrium phases are based on.</param>
    /// <returns>the height of this harmonic constituent</returns>
    public double Height (double hours)
    {
      return Amplitude * Math.Cos(DegreesToRadians(Speed * hours + EquilibriumPhase - Phase));
    }

    /// <summary>
    /// convert degrees to radians
    /// </summary>
    /// <param name="degrees">the degrees to convert</param>
    /// <returns>the radians</returns>
    public static double DegreesToRadians(double degrees)
    {
      return degrees * Math.PI / 180.0;
    }
  }
}
