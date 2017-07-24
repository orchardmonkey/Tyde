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
    /// <summary>
    /// Principal lunar semidiurnal
    /// </summary>
    public const int M2 = 0;

    /// <summary>
    /// Principal solar semidiurnal
    /// </summary>
    public const int S2 = 1;

    /// <summary>
    /// Larger lunar elliptic semidiurnal
    /// </summary>
    public const int N2 = 2;

    /// <summary>
    /// Lunar diurnal
    /// </summary>
    public const int K1 = 3;

    /// <summary>
    /// Shallow water overtides of principal lunar
    /// </summary>
    public const int M4 = 4;

    /// <summary>
    /// Lunar diurnal
    /// </summary>
    public const int O1 = 5;

    /// <summary>
    /// Shallow water overtides of principal lunar
    /// </summary>
    public const int M6 = 6;

    /// <summary>
    /// Shallow water terdiurnal
    /// </summary>
    public const int MK3 = 7;

    /// <summary>
    /// Shallow water overtides of principal solar
    /// </summary>
    public const int S4 = 8;

    /// <summary>
    /// Shallow water quarter dirunal
    /// </summary>
    public const int MN4 = 9;

    /// <summary>
    /// Larger lunar evectional
    /// </summary>
    public const int NU2 = 10;

    /// <summary>
    /// Shallow water overtides of principal solar
    /// </summary>
    public const int S6 = 11;

    /// <summary>
    /// Variational constituent
    /// </summary>
    public const int MU2 = 12;

    /// <summary>
    /// Lunar elliptical semidiurnal second-order
    /// </summary>
    public const int _2N2 = 13;

    /// <summary>
    /// Lunar diurnal
    /// </summary>
    public const int OO1 = 14;

    /// <summary>
    /// Smaller lunar evectional
    /// </summary>
    public const int LAMBDA2 = 15;

    /// <summary>
    /// Solar diurnal
    /// </summary>
    public const int S1 = 16;

    /// <summary>
    /// Smaller lunar elliptic diurnal
    /// </summary>
    public const int M1 = 17;

    /// <summary>
    /// Smaller lunar elliptic diurnal
    /// </summary>
    public const int J1 = 18;

    /// <summary>
    /// Lunar monthly
    /// </summary>
    public const int MM = 19;

    /// <summary>
    /// Solar semiannual
    /// </summary>
    public const int SSA = 20;

    /// <summary>
    /// Solar annual
    /// </summary>
    public const int SA = 21;

    /// <summary>
    /// Lunisolar synodic fortnightly
    /// </summary>
    public const int MSF = 22;

    /// <summary>
    /// Lunicolar fortnightly
    /// </summary>
    public const int MF = 23;

    /// <summary>
    /// Larger lunar evectional diurnal
    /// </summary>
    public const int RHO1 = 24;

    /// <summary>
    /// Larger lunar elliptic diurnal
    /// </summary>
    public const int Q1 = 25;

    /// <summary>
    /// Larger solar elliptic
    /// </summary>
    public const int T2 = 26;

    /// <summary>
    /// Smaller solar elliptic
    /// </summary>
    public const int R2 = 27;

    /// <summary>
    /// Larger elliptic diurnal
    /// </summary>
    public const int _2Q1 = 28;

    /// <summary>
    /// Solar diurnal 
    /// </summary>
    public const int P1 = 29;

    /// <summary>
    /// Shallow water semidiurnal
    /// </summary>
    public const int _2SM2 = 30;

    /// <summary>
    /// Lunar terdiurnal
    /// </summary>
    public const int M3 = 31;

    /// <summary>
    /// Smaller lunar elliptic semidiurnal
    /// </summary>
    public const int L2 = 32;

    /// <summary>
    /// Shallow water terdiurnal
    /// </summary>
    public const int _2MK3 = 33;

    /// <summary>
    /// Lunisolar semidiurnal
    /// </summary>
    public const int K2 = 34;

    /// <summary>
    /// Shallow water eighth diurnal
    /// </summary>
    public const int M8 = 35;

    /// <summary>
    /// Shallow water duarter diurnal
    /// </summary>
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
