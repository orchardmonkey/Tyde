using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{

  // NOAA's published tidal harmonic constituent phases and amplitudes for a tide station don't change from year to year.  
  // These harmonic constituent phases are meant to be aggregated with (actually subtracted from) the constituent phases of the Theoretical Equilibrium Tide which do change, depending upon the epoch.
  // The Theoritical Equilibrium Tide is the tide as it would exist if there were no continents, or continental shelves, just ocean.  
  // In this case, the tidal bulge would be directly under the moon, or more exactly, it would be perfectly related to astronomical phenomena.
  // The constituent phases of the Theoretical Equilibrium Tide can be calculated directly from astronomical data, for the start date of the epoch we wish to calculate tides for.
  // The constituent phases are calculated for some date and time (GMT), for the Theoretical Equilibrium Tide that would exist at 0 longitude.
  // Additionally, the constituent amplitudes published by NOAA for a tide station are meant to be modified by a node factor which is generally calculated once for an entire calendar year.  
  // The node factor represents the precession of the moon's node.  The moon's node is the point (actually there are two) where the plane of the moon's orbit intersects the plane of the ecliptic.
  // The plane of the moon's orbit slowly wobbles over a 18.6? year period, and this has an effect on its pull, weakening or strengthening it, and the constructive/destructive patten created with the sun's pull.
  // Trying not to use constructs like DateTime, so this can easily translate to C for an embedded system, or to javascript.
  class EquilibriumTide
  {
    public HarmonicConstituent[] Constituents { get; set; }

      // don't use constructs like DateTime, for easy translation to C
    public EquilibriumTide(int epochStartYear, int epochStartMonth, int epochStartDay, int epochLengthInDays)
    {
      this.Constituents = HarmonicConstituent.GetInitializedConstituents();

      int epochLengthInHours = epochLengthInDays * 24;  //RHRS


      // first determine the julian day of the beginning of the epoch.  Simply a one relative count into the current year.
      int epochStartJulianDay = GetJulianDay(epochStartYear, epochStartMonth, epochStartDay);  //DAYJ

      // determine node factors at middle of epoch
      CalculateNodeFactors(epochStartYear, epochStartJulianDay, epochLengthInHours / 2);

      // calculate the equlibrium phase for each harmonic constituent, for the beginning of the epoch
      CalculateEquilibriumPhases(epochStartYear, epochStartJulianDay, 0, epochLengthInHours / 2);

    }

    private void CalculateNodeFactors(int epochStartYear, int epochStartJulianDay, int halfEpochLengthInHours)
    {
      CelestialLongitudes middleOfEpochLongitudes = new CelestialLongitudes(epochStartYear, epochStartJulianDay, halfEpochLengthInHours);

      ///CalculateCelestialLongitudes(epochStartYear, epochStartJulianDay, halfEpochLengthInHours);
      double N = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.LongitudeOfMoonsNode); 
      double I = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DI);
      double NU = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DNU); 
      double XI = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DXI); 
      double P = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.LongitudeOfLunarPerigee); 
      double PC = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DPC); 
      double SINI = Math.Sin(I);
      double SINI2 = Math.Sin(I / 2.0);
      double SIN2I = Math.Sin(2.0 * I);
      double COSI2 = Math.Cos(I / 2.0);
      double TANI2 = Math.Tan(I / 2.0);
      // EQUATION 197, SCHUREMAN
      double QAINV = Math.Sqrt(2.310 + 1.435 * Math.Cos(2.0 * PC));
      // EQUATION 213, SCHUREMAN
      double RAINV = Math.Sqrt(1.0 - 12.0 * TANI2 * TANI2 * Math.Cos(2.0 * PC) + 36.0 * TANI2 * TANI2 * TANI2 * TANI2);
      // VARIABLE NAMES REFER TO EQUATION NUMBERS IN SCHUREMAN
      double EQ73 = (2.0 / 3.0 - SINI * SINI) / 0.5021;
      double EQ74 = SINI * SINI / 0.1578;
      double EQ75 = SINI * COSI2 * COSI2 / 0.37988;
      double EQ76 = Math.Sin(2.0 * I) / 0.7214;
      double EQ77 = SINI * SINI2 * SINI2 / 0.0164;
      double EQ78 = (COSI2 * COSI2 * COSI2 * COSI2) / 0.91544;
      double EQ149 = COSI2 * COSI2 * COSI2 * COSI2 * COSI2 * COSI2 / 0.8758;
      double EQ207 = EQ75 * QAINV;
      double EQ215 = EQ78 * RAINV;
      double EQ227 = Math.Sqrt(0.8965 * SIN2I * SIN2I + 0.6001 * SIN2I * Math.Cos(NU) + 0.1006);
      double EQ235 = 0.001 + Math.Sqrt(19.0444 * SINI * SINI * SINI * SINI + 2.7702 * SINI * SINI * Math.Cos(2.0 * NU) + 0.0981);
      // NODE FACTORS FOR 37 CONSTITUENTS:
      this.Constituents[Constants.M2].NodeFactor = EQ78;
      this.Constituents[Constants.S2].NodeFactor = 1.0;
      this.Constituents[Constants.N2].NodeFactor = EQ78;
      this.Constituents[Constants.K1].NodeFactor = EQ227;
      this.Constituents[Constants.M4].NodeFactor = EQ78 * EQ78;
      this.Constituents[Constants.O1].NodeFactor = EQ75;
      this.Constituents[Constants.M6].NodeFactor = EQ78 * EQ78 * EQ78;
      this.Constituents[Constants.MK3].NodeFactor = EQ78 * EQ227;
      this.Constituents[Constants.S4].NodeFactor = 1.0;
      this.Constituents[Constants.MN4].NodeFactor = EQ78 * EQ78;
      this.Constituents[Constants.NU2].NodeFactor = EQ78;
      this.Constituents[Constants.S6].NodeFactor = 1.0;
      this.Constituents[Constants.MU2].NodeFactor = EQ78;
      this.Constituents[Constants._2N2].NodeFactor = EQ78;
      this.Constituents[Constants.OO1].NodeFactor = EQ77;
      this.Constituents[Constants.LAMBDA2].NodeFactor = EQ78;
      this.Constituents[Constants.S1].NodeFactor = 1.0;
      //**EQUATION 207 NOT PRODUCING CORRECT ANSWER FOR M1
      //**SET NODE FACTOR FOR M1 = 1 UNTIL CAN FURTHER RESEARCH
      //this.Constituents[Constants.M1].NodeFactor = EQ207;
      this.Constituents[Constants.M1].NodeFactor = 1.0;
      this.Constituents[Constants.J1].NodeFactor = EQ76;
      this.Constituents[Constants.MM].NodeFactor = EQ73;
      this.Constituents[Constants.SSA].NodeFactor = 1.0;
      this.Constituents[Constants.SA].NodeFactor = 1.0;
      this.Constituents[Constants.MSF].NodeFactor = EQ78;
      this.Constituents[Constants.MF].NodeFactor = EQ74;
      this.Constituents[Constants.RHO1].NodeFactor = EQ75;
      this.Constituents[Constants.Q1].NodeFactor = EQ75;
      this.Constituents[Constants.T2].NodeFactor = 1.0;
      this.Constituents[Constants.R2].NodeFactor = 1.0;
      this.Constituents[Constants._2Q1].NodeFactor = EQ75;
      this.Constituents[Constants.P1].NodeFactor = 1.0;
      this.Constituents[Constants._2SM2].NodeFactor = EQ78;
      this.Constituents[Constants.M3].NodeFactor = EQ149;
      //**EQUATION 215 NOT PRODUCING CORRECT ANSWER FOR L2
      //**SET NODE FACTOR FOR L2 = 1 UNTIL CAN FURTHER RESEARCH
      //this.Constituents[Constants.L2].NodeFactor = EQ215;
      this.Constituents[Constants.L2].NodeFactor = 1.0;
      this.Constituents[Constants._2MK3].NodeFactor = EQ78 * EQ78 * EQ227;
      this.Constituents[Constants.K2].NodeFactor = EQ235;
      this.Constituents[Constants.M8].NodeFactor = EQ78 * EQ78 * EQ78 * EQ78;
      this.Constituents[Constants.MS4].NodeFactor = EQ78;
    }

    private void CalculateEquilibriumPhases(int epochStartYear, int epochStartJulianDay, int epochStartHour, int halfEpochLengthInHours)
    {
      // get celestial longitudes for start of the epoch
      CelestialLongitudes startOfEpochLongitudes = new CelestialLongitudes(epochStartYear, epochStartJulianDay, epochStartHour);
      double S = startOfEpochLongitudes.LongitudeOfMoon;
      double P = startOfEpochLongitudes.LongitudeOfLunarPerigee;
      double H = startOfEpochLongitudes.LongitudeOfSun;
      double P1 = startOfEpochLongitudes.LongitudeOfSolarPerigee;
      double T = MathFuncs.Angle(180.0 + epochStartHour * (360.0 / 24.0)); // hour angle of mean sun at place of observation

      // get values based on longitude of moon's node, these are calculated from the middle of the epoch.
      CelestialLongitudes middleOfEpochLongitudes = new CelestialLongitudes(epochStartYear, epochStartJulianDay, halfEpochLengthInHours);
      double NU = middleOfEpochLongitudes.DNU; //v
      double XI = middleOfEpochLongitudes.DXI; //e
      double NUP = middleOfEpochLongitudes.DNUP;
      double NUP2 = middleOfEpochLongitudes.DNUP2;

      // now fill in the equilibrium phases of the Harmonic Constituents.
      this.Constituents[Constants.M2].EquilibriumPhase = MathFuncs.Angle(2.0 * (T - S + H) + 2.0 * (XI - NU));
      this.Constituents[Constants.S2].EquilibriumPhase = MathFuncs.Angle(2.0 * T);
      this.Constituents[Constants.N2].EquilibriumPhase = MathFuncs.Angle(2.0 * (T + H) - 3.0 * S + P + 2.0 * (XI - NU));
      this.Constituents[Constants.K1].EquilibriumPhase = MathFuncs.Angle(T + H - 90.0 - NUP);
      this.Constituents[Constants.M4].EquilibriumPhase = MathFuncs.Angle(4.0 * (T - S + H) + 4.0 * (XI - NU));
      this.Constituents[Constants.O1].EquilibriumPhase = MathFuncs.Angle(T - 2.0 * S + H + 90.0 + 2.0 * XI - NU);
      this.Constituents[Constants.M6].EquilibriumPhase = MathFuncs.Angle(6.0 * (T - S + H) + 6.0 * (XI - NU));
      this.Constituents[Constants.MK3].EquilibriumPhase = MathFuncs.Angle(3.0 * (T + H) - 2.0 * S - 90.0 + 2.0 * (XI - NU) - NUP);
      this.Constituents[Constants.S4].EquilibriumPhase = MathFuncs.Angle(4.0 * T);
      this.Constituents[Constants.MN4].EquilibriumPhase = MathFuncs.Angle(4.0 * (T + H) - 5.0 * S + P + 4.0 * (XI - NU));
      this.Constituents[Constants.NU2].EquilibriumPhase = 2.0 * T - 3.0 * S + 4.0 * H - P + 2.0 * (XI - NU);
      this.Constituents[Constants.S6].EquilibriumPhase = 6.0 * T;
      this.Constituents[Constants.MU2].EquilibriumPhase = 2.0 * (T + 2.0 * (H - S)) + 2.0 * (XI - NU);
      this.Constituents[Constants._2N2].EquilibriumPhase = 2.0 * (T - 2.0 * S + H + P) + 2.0 * (XI - NU);
      this.Constituents[Constants.OO1].EquilibriumPhase = T + 2.0 * S + H - 90.0 - 2.0 * XI - NU;
      this.Constituents[Constants.LAMBDA2].EquilibriumPhase = 2.0 * T - S + P + 180.0 + 2.0 * (XI - NU);
      this.Constituents[Constants.S1].EquilibriumPhase = T;
      double I = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DI); 
      double PC = MathFuncs.DegreesToRadians(middleOfEpochLongitudes.DPC); 
      double TOP = (5.0 * Math.Cos(I) - 1.0) * Math.Sin(PC);
      double BOTTOM = (7.0 * Math.Cos(I) + 1.0) * Math.Cos(PC);
      double Q = MathFuncs.Arctan(TOP, BOTTOM, 1);
      this.Constituents[Constants.M1].EquilibriumPhase = T - S + H - 90.0 + XI - NU + Q;
      this.Constituents[Constants.J1].EquilibriumPhase = T + S + H - P - 90.0 - NU;
      this.Constituents[Constants.MM].EquilibriumPhase = S - P;
      this.Constituents[Constants.SSA].EquilibriumPhase = 2.0 * H;
      this.Constituents[Constants.SA].EquilibriumPhase = H;
      this.Constituents[Constants.MSF].EquilibriumPhase = 2.0 * (S - H);
      this.Constituents[Constants.MF].EquilibriumPhase = 2.0 * S - 2.0 * XI;
      this.Constituents[Constants.RHO1].EquilibriumPhase = T + 3.0 * (H - S) - P + 90.0 + 2.0 * XI - NU;
      this.Constituents[Constants.Q1].EquilibriumPhase = T - 3.0 * S + H + P + 90.0 + 2.0 * XI - NU;
      this.Constituents[Constants.T2].EquilibriumPhase = 2.0 * T - H + P1;
      this.Constituents[Constants.R2].EquilibriumPhase = 2.0 * T + H - P1 + 180.0;
      this.Constituents[Constants._2Q1].EquilibriumPhase = T - 4.0 * S + H + 2.0 * P + 90.0 + 2.0 * XI - NU;
      this.Constituents[Constants.P1].EquilibriumPhase = T - H + 90.0;
      this.Constituents[Constants._2SM2].EquilibriumPhase = 2.0 * (T + S - H) + 2.0 * (NU - XI);
      this.Constituents[Constants.M3].EquilibriumPhase = 3.0 * (T - S + H) + 3.0 * (XI - NU);
      double R = Math.Sin(2.0 * PC) / ((1.0 / 6.0) * (1.0 / Math.Tan(0.5 * I)) * (1.0 / Math.Tan(0.5 * I)) - Math.Cos(2.0 * PC));
      R = MathFuncs.RadiansToDegrees(Math.Atan(R)); 
      this.Constituents[Constants.L2].EquilibriumPhase = 2.0 * (T + H) - S - P + 180.0 + 2.0 * (XI - NU) - R;
      this.Constituents[Constants._2MK3].EquilibriumPhase = 3.0 * (T + H) - 4.0 * S + 90.0 + 4.0 * (XI - NU) + NUP;
      this.Constituents[Constants.K2].EquilibriumPhase = 2.0 * (T + H) - 2.0 * NUP2;
      this.Constituents[Constants.M8].EquilibriumPhase = 8.0 * (T - S + H) + 8.0 * (XI - NU);
      this.Constituents[Constants.MS4].EquilibriumPhase = 2.0 * (2.0 * T - S + H) + 2.0 * (XI - NU);

      for (int index = 0; index < Constants.ConstitutentArraySize; index++)
      {
        this.Constituents[index].EquilibriumPhase = MathFuncs.Angle(this.Constituents[index].EquilibriumPhase);
      }
    }

    // don't use constructs like DateTime, for easy translation to C
    private static int GetJulianDay(int epochStartYear, int epochStartMonth, int epochStartDay)
    {
      epochStartMonth = epochStartMonth - 1; // convert 1 relative to zero relative for array index;
      epochStartMonth = Math.Abs(epochStartMonth); // force positive
      epochStartMonth = epochStartMonth % 12; // force value 0-11

      int[] julianDaysBeforeMonth = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };

      int julianDays = julianDaysBeforeMonth[epochStartMonth] + epochStartDay;

      // if leap year and month greater than February (epochStartMonth is 0 relative now)
      // then add leap day that was missing from julianDaysBeforeMonth array
      if (epochStartYear % 4 == 0 && epochStartMonth > 1)
      {
        julianDays = julianDays + 1;
      }

      return julianDays;
    }
  }
}
