using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyde
{
  public class HarmonicConstituents
  {
    public HarmonicConstituent[] Constituents { get; set; }

    /// <summary>
    /// offset if harmonic constants are a little bit off from real tide.
    /// negative causes tidal events will be predicted to occur at a later time.
    /// </summary>
    public double HourlyOffset { get; set; }

    /// <summary>
    /// added to sum of all constituents
    /// </summary>
    public double MeanLowerLowWater { get; set; }

    /// <summary>
    /// year of epoch start
    /// </summary>
    public int EpochStartGMTYear { get; private set; }

    /// <summary>
    /// month of epoch start
    /// </summary>
    public int EpochStartGMTMonth { get; private set; }

    /// <summary>
    /// day of epoch start
    /// </summary>
    public int EpochStartGMTDay { get; private set; }

    /// <summary>
    /// length of epoch in days
    /// </summary>
    public int EpochLengthInDays { get; private set; }

    /// <summary>
    /// initialize the harmonic constituents with Speeds (always constant)
    /// and initialize with NodeFactors and EquilibriumPhases (calculated for the epoch start)
    /// </summary>
    public HarmonicConstituents(int epochStartGMTYear, int epochStartGMTMonth, int epochStartGMTDay, int epochLengthInDays)
    {
      this.EpochStartGMTYear = epochStartGMTYear;
      this.EpochStartGMTMonth = epochStartGMTMonth;
      this.EpochStartGMTDay = epochStartGMTDay;
      this.EpochLengthInDays = epochLengthInDays;
      this.HourlyOffset = 0;
      this.MeanLowerLowWater = 8.33;

      EquilibriumTide equilibriumTide = new EquilibriumTide(epochStartGMTYear, epochStartGMTMonth, epochStartGMTDay, epochLengthInDays);
      this.Constituents = equilibriumTide.Constituents;
    }

    public List<double> FindTimesForHeight(double height, double hoursSinceEpochStart, double hoursToSearch)
    {
      HashSet<double> foundHours = new HashSet<double>();

      // search every 2 hours
      for (double searchHour = hoursSinceEpochStart; searchHour < hoursSinceEpochStart + hoursToSearch; searchHour += 2)
      {
        double? foundHour = this.FindTimeForHeight(height, searchHour);
        //double? sevenTime = this.constituents.FindTimeForRateOfChange(0, HoursSinceEpochStart(startOfToday + new TimeSpan(i * 2, 0, 0)));
        if (foundHour.HasValue)
        {
          // truncate to two decimal places to make it easier to drop duplicates.
          // truncating to two decimal places is approimately truncating to the nearest minute.
          foundHour = ((double)((int)(foundHour.Value * 100))) / 100.0;

          if (foundHour >= hoursSinceEpochStart && foundHour <= hoursSinceEpochStart + hoursToSearch)
          {
            foundHours.Add(foundHour.Value);
          }
        }
      }
      return foundHours.OrderBy(hour => hour).ToList();
    }


    public List<double> FindTimesForRateOfChange(double rateOfchange, double hoursSinceEpochStart, double hoursToSearch)
    {
      HashSet<double> foundHours = new HashSet<double>();

      // search every 2 hours
      for (double searchHour = hoursSinceEpochStart; searchHour < hoursSinceEpochStart + hoursToSearch; searchHour += 2)
      {
        double? foundHour = this.FindTimeForRateOfChange(rateOfchange, searchHour);
        //double? sevenTime = this.constituents.FindTimeForRateOfChange(0, HoursSinceEpochStart(startOfToday + new TimeSpan(i * 2, 0, 0)));
        if (foundHour.HasValue)
        {
          // truncate to two decimal places to make it easier to drop duplicates.
          // truncating to two decimal places is approimately truncating to the nearest minute.
          foundHour = ((double)((int)(foundHour.Value * 100))) / 100.0;

          if (foundHour >= hoursSinceEpochStart && foundHour <= hoursSinceEpochStart + hoursToSearch)
          {
            foundHours.Add(foundHour.Value);
          }
        }
      }
      return foundHours.OrderBy(hour => hour).ToList();
    }


    /// <summary>
    /// use Newton's method to solve for zero.
    /// basically, our next x value is current x - f(x)/f1(x)
    /// where f1(x) is the derivative of f(x) with respect to x
    /// in this case, f1(x) is PredictAcceleration()
    /// and f(x) is PredictRateOfChange() - targetRateOfChange
    /// basically, if targetRateOfChange is zero, then we are looking for slack tide within 3 hours of the passed hoursSinceEpochStart
    /// </summary>
    /// <param name="targetRateOfChange">Rate of change we are looking for</param>
    /// <param name="hoursSinceEpochStart">number of hours since epoch start to begin search at</param>
    /// <returns>number of hours since epoch start where target rate of change occurred, or null</returns>
    public double? FindTimeForRateOfChange(double targetRateOfChange, double hoursSinceEpochStart)
    {
      double solutionHoursSinceEpochStart = hoursSinceEpochStart;

      for (int i = 0; i < 5; i++)
      {
        double startingRateOfChange = PredictRateOfChange(solutionHoursSinceEpochStart);
        double startingSlope = PredictAccelerationOfChange(solutionHoursSinceEpochStart);
        solutionHoursSinceEpochStart = solutionHoursSinceEpochStart - (startingRateOfChange - targetRateOfChange) / startingSlope;
      }

      double endRateOfChange = PredictRateOfChange(solutionHoursSinceEpochStart);
      if (endRateOfChange - targetRateOfChange > .05 || endRateOfChange - targetRateOfChange < -.05)
      {
        return null;
      }

      // sometimes if our start time is too close a time of zero acceleration, then the slope is almost zero.
      // so when we divide by the slope to get our next x, we jump many days - returning a value that is too far off.
      if (solutionHoursSinceEpochStart - hoursSinceEpochStart > 3.0 || hoursSinceEpochStart - solutionHoursSinceEpochStart > 3.0)
      {
        return null;
      }

      return solutionHoursSinceEpochStart;
    }

    /// <summary>
    /// use Newton's method to solve for zero.
    /// basically, our next x value is current x - f(x)/f1(x)
    /// where f1(x) is the derivative of f(x) with respect to x
    /// in this case, f1(x) is PredictRateOfChange()
    /// and f(x) is PredictTideHeight() - targetHeight
    /// basically, we are looking for the specific time a tide of X feet occurred, within 3 hours of the passed hoursSinceEpochStart
    /// </summary>
    /// <param name="targetHeight">height of tide we wish to calculate occurrence time for</param>
    /// <param name="hoursSinceEpochStart">the number of hours since epoch start to start solving for</param>
    /// <returns>number of hours since epoch start when tide of targetHeight occurred, or null</returns>
    public double? FindTimeForHeight(double targetHeight, double hoursSinceEpochStart)
    {
      double solutionHoursSinceEpochStart = hoursSinceEpochStart;

      for (int i = 0; i < 5; i++)
      {
        double startingHeight = PredictTideHeight(solutionHoursSinceEpochStart);
        double startingSlope = PredictRateOfChange(solutionHoursSinceEpochStart);
        solutionHoursSinceEpochStart = solutionHoursSinceEpochStart - (startingHeight - targetHeight) / startingSlope;
      }

      double endHeight = PredictTideHeight(solutionHoursSinceEpochStart);
      if (endHeight - targetHeight > .05 || endHeight - targetHeight < -.05)
      {
        return null;
      }

      // sometimes if our start time is too close to slack tide, then the slope is almost zero.
      // so when we divide by the slope to get our next x, we jump many days - returning a value that is too far off.
      if (solutionHoursSinceEpochStart - hoursSinceEpochStart > 3 || hoursSinceEpochStart - solutionHoursSinceEpochStart > 3)
      {
        return null;
      }

      return solutionHoursSinceEpochStart;
    }


    /// <summary>
    /// predict height of Tide by summing the height of the constituents for a given number of hours since epoch start.  
    /// </summary>
    /// <param name="dateTime">local time that we wish to calculate Olympia tides for</param>
    /// <returns>height of tide</returns>
    public double PredictTideHeight(double hoursSinceEpochStart)
    {
      return this.Constituents.Sum(c => c.Height(hoursSinceEpochStart + this.HourlyOffset)) + this.MeanLowerLowWater;
    }


    public double PredictRateOfChange(double hoursSinceEpochStart)
    {
      return this.Constituents.Sum(c => c.RateOfChange(hoursSinceEpochStart + this.HourlyOffset));
    }

    public double PredictAccelerationOfChange(double hoursSinceEpochStart)
    {
      return this.Constituents.Sum(c => c.AccelerationOfChange(hoursSinceEpochStart + this.HourlyOffset));
    }
  }
}
