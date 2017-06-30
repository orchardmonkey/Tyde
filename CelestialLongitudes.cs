using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  class CelestialLongitudes
  {
    /// <summary>
    /// Longitude of the Moon's node in degrees
    /// </summary>
    public double LongitudeOfMoonsNode { get; private set; }

    /// <summary>
    /// Longitude of Moon in degrees
    /// </summary>
    public double LongitudeOfMoon { get; private set; }

    /// <summary>
    /// Longitude of the Sun in degrees
    /// </summary>
    public double LongitudeOfSun { get; private set; }

    /// <summary>
    /// Longitude of Lunar perigee in degrees
    /// </summary>
    public double LongitudeOfLunarPerigee { get; private set; }

    /// <summary>
    /// Longitude of Solar perigee in degrees
    /// </summary>
    public double LongitudeOfSolarPerigee { get; private set; }


    /// <summary>
    /// trig based on longitude of Moon's node.
    /// </summary>
    public double DI { get; private set; }


    /// <summary>
    /// trig based on longitude of Moon's node.
    /// </summary>
    public double DNU { get; private set; }


    /// <summary>
    /// trig based on longitude of Moon's node.
    /// </summary>
    public double DXI { get; private set; }


    /// <summary>
    /// trig based on longitude of Moon's node and longitude of Lunar Perigee.
    /// </summary>
    public double DPC { get; private set; }

    /// <summary>
    /// trig based on longitude of Moon's node
    /// </summary>
    public double DNUP { get; private set; }

    /// <summary>
    /// trig based on longitude of Moon's node
    /// </summary>
    public double DNUP2 { get; private set; }


    public CelestialLongitudes(int year, int julianDay, int hours)
    {
      double doubleHour = hours;
      int leapDaysSince1901 = ((year - 1901) / 4); //number of leap days between 1901 and Jan 1 of year
      double yearsSince1900 = year - 1900; //number of years since 1900
      double additionalDays = julianDay + leapDaysSince1901 - 1;  // julian Day + leap days.

      this.LongitudeOfMoonsNode = 259.1560564 - 19.328185764 * yearsSince1900 - .0529539336 * additionalDays - .0022064139 * doubleHour;
      this.LongitudeOfMoonsNode = MathFuncs.Angle(LongitudeOfMoonsNode);

      this.LongitudeOfLunarPerigee = 334.3837214 + 40.66246584 * yearsSince1900 + .111404016 * additionalDays + .004641834 * doubleHour;
      this.LongitudeOfLunarPerigee = MathFuncs.Angle(this.LongitudeOfLunarPerigee);

      this.LongitudeOfSun = 280.1895014 - .238724988 * yearsSince1900 + .9856473288 * additionalDays + .0410686387 * doubleHour;
      this.LongitudeOfSun = MathFuncs.Angle(LongitudeOfSun);

      this.LongitudeOfSolarPerigee = 281.2208569 + .01717836 * yearsSince1900 + .000047064 * additionalDays + .000001961 * doubleHour;
      this.LongitudeOfSolarPerigee = MathFuncs.Angle(LongitudeOfSolarPerigee);

      this.LongitudeOfMoon = 277.0256206 + 129.38482032 * yearsSince1900 + 13.176396768 * additionalDays + .549016532 * doubleHour;
      this.LongitudeOfMoon = MathFuncs.Angle(LongitudeOfMoon);

      double N = MathFuncs.DegreesToRadians(LongitudeOfMoonsNode);  
      double I = Math.Acos(.9136949 - .0356926 * Math.Cos(N));  
      this.DI = MathFuncs.Angle(MathFuncs.RadiansToDegrees(I));  

      double NU = Math.Asin(.0897056 * Math.Sin(N) / Math.Sin(I)); 
      this.DNU = MathFuncs.RadiansToDegrees(NU); //v

      double XI = N - 2.0 * Math.Atan(.64412 * Math.Tan(N / 2.0)) - NU;  
      this.DXI = MathFuncs.RadiansToDegrees(XI); //e

      double NUP = Math.Atan(Math.Sin(NU) / (Math.Cos(NU) + .334766 / Math.Sin(2.0 * I)));
      this.DNUP = MathFuncs.RadiansToDegrees(NUP); 

      double NUP2 = Math.Atan(Math.Sin(2.0 * NU) / (Math.Cos(2.0 * NU) + .0726184 / (Math.Sin(I) * Math.Sin(I)))) / 2.0;
      this.DNUP2 = MathFuncs.RadiansToDegrees(NUP2); 

      this.DPC = MathFuncs.Angle(this.LongitudeOfLunarPerigee - this.DXI);
    }


  }
}
