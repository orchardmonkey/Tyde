using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  /// <summary>
  /// put math stuff here for easy switch to C++ or Javascript or whaterver
  /// </summary>
  public static class MathFuncs
  {
    public static double Cos(double d)
    {
      return Math.Cos(d);
    }

    public static double Sin(double d)
    {
      return Math.Sin(d);
    }

    public static double Tan(double d)
    {
      return Math.Tan(d);
    }

    public static double Atan(double d)
    {
      return Math.Atan(d);
    }

    public static double Asin(double d)
    {
      return Math.Asin(d);
    }

    public static double Acos(double d)
    {
      return Math.Acos(d);
    }

    /// <summary>
    /// place the angle in the (+) 0-360 range
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static double Angle(double angle)
    {
      int quotient = -(int)(angle / 360.0);
      angle = angle + ((double)quotient) * 360.0; // basically like modulus 360
      if (angle < 0)
      {
        angle = angle + 360.0;
      }

      return angle;
    }

    /// <summary>
    /// convert degrees to radians
    /// </summary>
    /// <param name="degrees">the degrees to convert</param>
    /// <returns>the radians</returns>
    public static double DegreesToRadians(double degrees)
    {
      return degrees * (Math.PI / 180.0);
    }

    /// <summary>
    /// convert radians to degrees
    /// </summary>
    /// <param name="radians">the radians to convert</param>
    /// <returns>degrees</returns>
    public static double RadiansToDegrees(double radians)
    {
      return radians / ( Math.PI / 180.0 );
    }

    /// <summary>
    /// find the arctangent and place in the correct quadrant.
    /// </summary>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    /// <param name="key">0 = no selection made.  </param>
    /// <returns></returns>
    public static double Arctan(double top, double bottom, double key)
    {
      double arctan = 0;

      if (bottom == 0.0)
      {
        if (top < 0.0)
        {
          // 2
          arctan = 270;
          return arctan;
        }
        if (top == 0)
        {
          // 9
          arctan = 0;
          return arctan;
        }
        else
        {
          // 3
          arctan = 90;
          return arctan;
        }
      }
      else // if (bottom != 0.0)
      {
        // 4
        arctan = Math.Atan(top / bottom) * 57.2957795;
        if (key == 0.0)
        {
          return arctan;
        }
        if (top <= 0.0)
        {
          // 5
          if (bottom < 0)
          {
            // 6
            arctan = arctan + 180;
            return arctan;
          }
          else if (bottom == 0)
          {
            //9
            arctan = 0;
            return arctan;
          }
          else
          {
            //8
            arctan = arctan + 360;
            return arctan;
          }
        }
        else
        {
          // 7
          if (bottom < 0)
          {
            // 6
            arctan = arctan + 180;
            return arctan;
          }
          else if (bottom == 0)
          {
            // 3
            arctan = 90;
            return arctan;
          }
          else
          {
            // 10
            return arctan;
          }
        }
      }
    }

  }
}
