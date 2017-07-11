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
  /// TidalHeight = Sum of Height for all HarmonicConstituents + mean water level for locale.
  /// Hight of HarmonicConstituent = NodeFactor * Amplitude * Cos(Speed * t + EquilibriumPhase - Phase)
  /// where t is the number of hours elapsed since the start of the epoch that the EquilibriumPhase was calculated for.
  /// </summary>
  public class HarmonicConstituent
  {
    public HarmonicConstituent()
    {
      Amplitude = 0;
      Description = string.Empty;
      Phase = 0;
      EquilibriumPhase = 0;
      NodeFactor = 1;
      Speed = 0;
    }

    /// <summary>
    /// return an array of 37 harmonic constituents, initialized with descriptions and speeds.
    /// later we will update Phases and Amplitudes based on locale, and also update
    /// EquilibriumPhases and NodeFactors based on the theoretical equilibrium tide calculated for our prediction epoch.
    /// </summary>
    /// <returns>array of all harmonic constituents</returns>
    public static HarmonicConstituent[] GetInitializedConstituents()
    {
      HarmonicConstituent[] constituents = new HarmonicConstituent[Constants.ConstitutentArraySize];

      for (int i = 0; i < Constants.ConstitutentArraySize; i++)
      {
        constituents[i] = new HarmonicConstituent();
      }

      // Speeds are in degrees per hour.  For example, S2 completes a 360 degree cycle in 12 hours. (360/30 = 12)
      // T + h gives us rotation of earth with respect to fixed stars.  So T + h - s gives us the lunar day.  

      constituents[Constants.M2].ID = "M2";
      constituents[Constants.M2].Description = "Principal Lunar, Semi-Diurnal (1 cycle = 1/2 rotation of earth with respect to moon)";
      constituents[Constants.M2].Speed = 2.0 * Constants.T - 2.0 * Constants.s + 2.0 * Constants.h; // 28.9841042;

      constituents[Constants.S2].ID = "S2";
      constituents[Constants.S2].Description = "Principal Solar, Semi-Diurnal (1 cycle = 1/2 rotation of earth with respect to sun)";
      constituents[Constants.S2].Speed = 2.0 * Constants.T; // 30.0000000;

      constituents[Constants.N2].ID = "N2";
      constituents[Constants.N2].Description = "Principal lunar elliptic, semidiurnal (1 cycle = 1/2 rotation of earth with respect to moon's perigee)";
      constituents[Constants.N2].Speed = 2.0 * Constants.T - 3.0 * Constants.s + 2.0 * Constants.h + Constants.p;   // 28.4397295;

      constituents[Constants.K1].ID = "K1";
      constituents[Constants.K1].Description = "Combined Sun-Moon angle (1 cycle = full Rotation of earth with respect to fixed stars)";
      constituents[Constants.K1].Speed = Constants.T + Constants.h; // 15.0410686;

      constituents[Constants.M4].ID = "M4";
      constituents[Constants.M4].Description = "Shallow water"; 
      constituents[Constants.M4].Speed = 57.9682084;

      constituents[Constants.O1].ID = "O1";
      constituents[Constants.O1].Description = "Principal lunar declination";
      constituents[Constants.O1].Speed = Constants.T - 2.0 * Constants.s + Constants.h; // 13.9430356;

      constituents[Constants.M6].ID = "M6";
      constituents[Constants.M6].Description = "Shallow water";
      constituents[Constants.M6].Speed = 86.9523127;

      constituents[Constants.MK3].ID = "MK3";
      constituents[Constants.MK3].Description = "";
      constituents[Constants.MK3].Speed = 44.0251729;

      constituents[Constants.S4].ID = "S4";
      constituents[Constants.S4].Description = "";
      constituents[Constants.S4].Speed = 60.0;

      constituents[Constants.MN4].ID = "MN4";
      constituents[Constants.MN4].Description = "";
      constituents[Constants.MN4].Speed = 57.4238337;

      constituents[Constants.NU2].ID = "NU2";
      constituents[Constants.NU2].Description = "";
      constituents[Constants.NU2].Speed = 2.0 * Constants.T - 3.0 * Constants.s + 4.0 * Constants.h - Constants.p; // 28.5125831

      constituents[Constants.S6].ID = "S6";
      constituents[Constants.S6].Description = "";
      constituents[Constants.S6].Speed = 90.0;

      constituents[Constants.MU2].ID = "MU2";
      constituents[Constants.MU2].Description = "";
      constituents[Constants.MU2].Speed = 27.9682084;

      constituents[Constants._2N2].ID = "2N2";
      constituents[Constants._2N2].Description = "";
      constituents[Constants._2N2].Speed = 27.8953548;

      constituents[Constants.OO1].ID = "OO1";
      constituents[Constants.OO1].Description = "";
      constituents[Constants.OO1].Speed = 16.1391017;

      constituents[Constants.LAMBDA2].ID = "LAMBDA2";
      constituents[Constants.LAMBDA2].Description = "";
      constituents[Constants.LAMBDA2].Speed = 29.4556253;

      constituents[Constants.S1].ID = "S1";
      constituents[Constants.S1].Description = "";
      constituents[Constants.S1].Speed = 15.0;

      constituents[Constants.M1].ID = "M1";
      constituents[Constants.M1].Description = "";
      constituents[Constants.M1].Speed = 14.4966939;

      constituents[Constants.J1].ID = "J1";
      constituents[Constants.J1].Description = "";
      constituents[Constants.J1].Speed = 15.5854433;

      constituents[Constants.MM].ID = "MM";
      constituents[Constants.MM].Description = "precession of Moon's perigee with respect to Moon's phase?";
      constituents[Constants.MM].Speed = Constants.s - Constants.p; // 0.5443747

      constituents[Constants.SSA].ID = "SSA";
      constituents[Constants.SSA].Description = "";
      constituents[Constants.SSA].Speed = 0.0821373;

      constituents[Constants.SA].ID = "SA";
      constituents[Constants.SA].Description = "Earth's Perihelion";
      constituents[Constants.SA].Speed = Constants.h; // 0.0410686

      constituents[Constants.MSF].ID = "MSF";
      constituents[Constants.MSF].Description = "";
      constituents[Constants.MSF].Speed = 1.0158958;

      constituents[Constants.MF].ID = "MF";
      constituents[Constants.MF].Description = "";
      constituents[Constants.MF].Speed = 1.0980331;

      constituents[Constants.RHO1].ID = "RHO1";
      constituents[Constants.RHO1].Description = "";
      constituents[Constants.RHO1].Speed = 13.4715145;

      constituents[Constants.Q1].ID = "Q1";
      constituents[Constants.Q1].Description = "";
      constituents[Constants.Q1].Speed = 13.3986609;

      constituents[Constants.T2].ID = "T2";
      constituents[Constants.T2].Description = "";
      constituents[Constants.T2].Speed = 29.9589333;

      constituents[Constants.R2].ID = "R2";
      constituents[Constants.R2].Description = "";
      constituents[Constants.R2].Speed = 30.0410667;

      constituents[Constants._2Q1].ID = "2Q1";
      constituents[Constants._2Q1].Description = "";
      constituents[Constants._2Q1].Speed = 12.8542862;

      constituents[Constants.P1].ID = "P1";
      constituents[Constants.P1].Description = "Principal solar declination";
      constituents[Constants.P1].Speed = Constants.T - Constants.h; // 14.9589314;

      constituents[Constants._2SM2].ID = "2SM2";
      constituents[Constants._2SM2].Description = "";
      constituents[Constants._2SM2].Speed = 31.0158958;

      constituents[Constants.M3].ID = "M3";
      constituents[Constants.M3].Description = "";
      constituents[Constants.M3].Speed = 43.4761563;

      constituents[Constants.L2].ID = "L2";
      constituents[Constants.L2].Description = "";
      constituents[Constants.L2].Speed = 2.0 * Constants.T - Constants.s + 2.0 * Constants.h - Constants.p; // 29.5284789

      constituents[Constants._2MK3].ID = "2MK3";
      constituents[Constants._2MK3].Description = "";
      constituents[Constants._2MK3].Speed = 42.9271398;

      constituents[Constants.K2].ID = "K2";
      constituents[Constants.K2].Description = "Combined Sun-Moon angle (1 cycle = 1/2 rotation of earth with respect to fixed stars)";
      constituents[Constants.K2].Speed = 2.0 * Constants.T + 2.0 * Constants.h; // 30.0821373

      constituents[Constants.M8].ID = "M8";
      constituents[Constants.M8].Description = "";
      constituents[Constants.M8].Speed = 115.9364169;

      constituents[Constants.MS4].ID = "MS4";
      constituents[Constants.MS4].Description = "";
      constituents[Constants.MS4].Speed = 58.9841042;

      return constituents;
    }


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
    /// Calculate the height of this constituent.  Same units (probably feet or meters) as the amplitude
    /// </summary>
    /// <param name="h">the number of hours since the start of the epoch that the equilibrium phases are based on.</param>
    /// <returns>the height of this harmonic constituent</returns>
    public double Height (double hours)
    {
      return Amplitude * NodeFactor * Math.Cos(MathFuncs.DegreesToRadians(Speed * hours + EquilibriumPhase - Phase));
    }

    /// <summary>
    /// this is the derivative of the Height function, it returns speed of the tide
    /// Same units (probably feet or meters) per hour as the amplitude
    /// </summary>
    /// <param name="hours">hours since epoch start</param>
    /// <returns>speed of tide</returns>
    public double RateOfChange (double hours)
    {
      return -1 * Amplitude * NodeFactor * (Math.PI/180) * Speed * Math.Sin(MathFuncs.DegreesToRadians(Speed * hours + EquilibriumPhase - Phase));
    }

    /// <summary>
    /// this is the derivative of the RateOfChange function, it returns the acceleration of the tide
    /// Same units (probably feet or meters) per hour per hour as the amplitude
    /// </summary>
    /// <param name="hours">hours since epoch start</param>
    /// <returns>the acceleration of the tide</returns>
    public double AccelerationOfChange(double hours)
    {
      return -1 * Amplitude * NodeFactor * (Math.PI / 180) * Speed * (Math.PI / 180) * Speed * Math.Cos(MathFuncs.DegreesToRadians(Speed * hours + EquilibriumPhase - Phase));
    }


  }
}
