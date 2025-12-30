using DeepSigma.General.Extensions;
using DeepSigma.PerformanceEngine.Enums;
using DeepSigma.General.TimeStepper;
using DeepSigma.PerformanceEngine.Models;
using DeepSigma.General.Enums;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General;

namespace DeepSigma.PerformanceEngine;

/// <summary>
/// Engine to calculate performance analytics from performance data points.
/// </summary>
public class PerformanceEngine
{
    private SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> PerformanceData { get; set; } = [];
    private SortedDictionary<DateOnlyCustom, decimal> PortfolioReturns { get; set; } = [];
    private SortedDictionary<DateOnlyCustom, decimal> BenchmarkReturns { get; set; } = [];

    /// <summary>
    /// Initialize the performance engine with performance data points.
    /// </summary>
    /// <param name="performance_data"></param>
    public PerformanceEngine(SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> performance_data)
    {
        this.PerformanceData = performance_data;
        this.PortfolioReturns = PerformanceData.GetExtractedPropertyAsSeries(x => x.PortfolioReturn).ToSortedDictionary();
        this.BenchmarkReturns = PerformanceData.GetExtractedPropertyAsSeries(x => x.BenchmarkReturn).ToSortedDictionary();
    }

    /// <summary>
    /// Calculate performance summary for standard time periods.
    /// </summary>
    /// <param name="as_of_date"></param>
    /// <returns></returns>
    public List<PerformanceAnalyticsResults<DateOnlyCustom>> CalculatePerformanceDataSummary(DateOnly as_of_date)
    {
        List<PerformanceAnalyticsResults<DateOnlyCustom>> results = [];
        foreach (PerformanceTimePeriod period in Enum.GetValues<PerformanceTimePeriod>())
        {
            DateOnlyCustom startDate = GetPeriodStartDate(PortfolioReturns.Keys.Min(), PortfolioReturns.Keys.Max(), period);
            DateOnlyCustom endDate = as_of_date;
            if(PortfolioReturns.Keys.Min() <= startDate)
            {
                SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> selected_data = this.PerformanceData.Where(x => x.Key <= endDate && x.Key >= startDate).ToSortedDictionary();
                PerformancePeriodAnalytics item = new(selected_data, period);
                
                var result = item.GetComputePeformanceForPeriod();
                results.Add(result);
            }
        }
        return results;
    }

    /// <summary>
    /// Calculate performance on intervals based on the specified periodicity.
    /// </summary>
    /// <param name="as_of_date"></param>
    /// <param name="periodicity"></param>
    /// <param name="performance_time_period"></param>
    /// <returns></returns>
    public List<PerformanceAnalyticsResults<DateOnlyCustom>> CalculatePerformanceForCustomPeriodicity(DateOnly as_of_date, Periodicity periodicity, PerformanceTimePeriod performance_time_period)
    {
        PeriodicityConfiguration config = new(periodicity);
        SelfAligningTimeStepper<DateOnlyCustom> time_step = new(config);

        DateOnly min_date = PortfolioReturns.Keys.Min();
        DateOnly selected_data_time = as_of_date;

        List<PerformanceAnalyticsResults<DateOnlyCustom>> results = [];
        while (selected_data_time >= min_date)
        {
            DateOnlyCustom TargetEndDate = selected_data_time;
            DateOnlyCustom TargetStartDate = time_step.GetPreviousTimeStep(selected_data_time).AddDays(1);
            SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> selected_data = this.PerformanceData.Where(x => x.Key <= TargetEndDate && x.Key >= TargetStartDate).ToSortedDictionary();
            PerformancePeriodAnalytics item = new(selected_data, performance_time_period);
            
            var result = item.GetComputePeformanceForPeriod();
            results.Add(result);
            selected_data_time = TargetStartDate.AddDays(-1);
        }
        return results;
    }

    /// <summary>
    /// Get the start date for a given performance time period.
    /// </summary>
    /// <param name="start_date"></param>
    /// <param name="end_date"></param>
    /// <param name="period"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static DateOnly GetPeriodStartDate(DateOnly start_date, DateOnly end_date, PerformanceTimePeriod period)
    {
        switch (period)
        {
            case (PerformanceTimePeriod.Day1):
                return end_date.AddWeekdays(-1);
            case (PerformanceTimePeriod.Day2):
                return end_date.AddWeekdays(-2);
            case (PerformanceTimePeriod.Day3):
                return end_date.AddWeekdays(-3);
            case (PerformanceTimePeriod.WTD):
                PeriodicityConfiguration weekly_config = new(Periodicity.Weekly);
                SelfAligningTimeStepper<DateOnlyCustom> weeklyTimeStep = new(weekly_config, required_day_of_week: DayOfWeek.Friday);
                return weeklyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
            case (PerformanceTimePeriod.MTD):
                PeriodicityConfiguration monthly_config = new(Periodicity.Monthly);
                SelfAligningTimeStepper<DateOnlyCustom> monthlyTimeStep = new(monthly_config);
                return monthlyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
            case (PerformanceTimePeriod.QTD):
                PeriodicityConfiguration quarterly_config = new(Periodicity.Quarterly);
                SelfAligningTimeStepper<DateOnlyCustom> quarterlyTimeStep = new(quarterly_config);
                return quarterlyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
            case (PerformanceTimePeriod.YTD):
                PeriodicityConfiguration annual_config = new(Periodicity.Annually);
                SelfAligningTimeStepper<DateOnlyCustom> yearlyTimeStep = new(annual_config);
                return yearlyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
            case (PerformanceTimePeriod.OneYear):
                return end_date.AddYears(-1).AddDays(1);
            case (PerformanceTimePeriod.TwoYear):
                return end_date.AddYears(-2).AddDays(1);
            case (PerformanceTimePeriod.ThreeYear):
                return end_date.AddYears(-3).AddDays(1);
            case (PerformanceTimePeriod.FiveYear):
                return end_date.AddYears(-5).AddDays(1);
            case (PerformanceTimePeriod.TenYear):
                return end_date.AddYears(-10).AddDays(1);
            case (PerformanceTimePeriod.ITD):
                return start_date;
            default:
                throw new NotImplementedException();
        }
    }
}
