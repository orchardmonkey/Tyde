using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  /// <summary>
  /// array indexes for harmonic constituents.
  /// and doodson numbers used to calculate speeds of the constituents.
  /// </summary>
  static class Constants
  {
    public const int M2 = 0;
    public const int S2 = 1;
    public const int N2 = 2;
    public const int K1 = 3;
    public const int M4 = 4;
    public const int O1 = 5;
    public const int M6 = 6;
    public const int MK3 = 7;
    public const int S4 = 8;
    public const int MN4 = 9;
    public const int NU2 = 10;
    public const int S6 = 11;
    public const int MU2 = 12;
    public const int _2N2 = 13;
    public const int OO1 = 14;
    public const int LAMBDA2 = 15;
    public const int S1 = 16;
    public const int M1 = 17;
    public const int J1 = 18;
    public const int MM = 19;
    public const int SSA = 20;
    public const int SA = 21;
    public const int MSF = 22;
    public const int MF = 23;
    public const int RHO1 = 24;
    public const int Q1 = 25;
    public const int T2 = 26;
    public const int R2 = 27;
    public const int _2Q1 = 28;
    public const int P1 = 29;
    public const int _2SM2 = 30;
    public const int M3 = 31;
    public const int L2 = 32;
    public const int _2MK3 = 33;
    public const int K2 = 34;
    public const int M8 = 35;
    public const int MS4 = 36;
    public const int ConstitutentArraySize = 37;

    // The next values are Doodson numbers, and are all that is necessary to calculate the speed in 
    // degrees per hour of each of the harmonic constituent waveforms.
    // https://en.wikipedia.org/wiki/Arthur_Thomas_Doodson

    /// <summary>
    /// Rotation of the Earth on its axis, with respect to the Sun, degrees per hour.
    /// </summary>
    public const double T = 15.0;

    /// <summary>
    /// Revolution of the Earth about the Sun, degrees per hour, with respect to fixed stars.
    /// </summary>
    public const double h = 0.04106864;

    /// <summary>
    /// Revolution of the Moon about the Earth, degrees per hour, with respect to fixed stars.
    /// </summary>
    public const double s = 0.54901653;

    /// <summary>
    /// Precession of the Moon's perigee, degrees per hour, with respect to fixed stars.
    /// </summary>
    public const double p = 0.00464183;

    /// <summary>
    /// Precession of the plane of the Moon's orbit, degrees per hour, with respect to fixed stars.
    /// </summary>
    public const double N = -0.00220641;
  }
}
