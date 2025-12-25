using DeepSigma.PerformanceEngine.Enums;
using DeepSigma.PerformanceEngine.Models;
using DeepSigma.Mathematics.Statistics;
using DeepSigma.General.Extensions;
using DeepSigma.General.DateTimeUnification;

namespace DeepSigma.PerformanceEngine;

/// <summary>
/// Class to compute performance analytics from performance data points.
/// </summary>
internal class PerformancePeriodAnalytics
{
    private PerformanceTimePeriod PerformanceTimePeriod { get; set; }
    private SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> PerformanceData { get; set; }

    /// <inheritdoc cref="PerformancePeriodAnalytics"/>
    internal PerformancePeriodAnalytics(SortedDictionary<DateOnlyCustom, PerformanceDataPoint<DateOnlyCustom>> performance_data, PerformanceTimePeriod time_period)
    {
        this.PerformanceData = performance_data;
        this.PerformanceTimePeriod = time_period;
    }

    /// <summary>
    /// Compute the performance analytics based on the performance data points.
    /// </summary>
    internal PerformanceAnalyticsResults<DateOnlyCustom> GetComputePeformanceForPeriod()
    {
        PerformanceAnalyticsResults<DateOnlyCustom> results = new()
        {
            StartDate = PerformanceData.Keys.Min(),
            EndDate = PerformanceData.Keys.Max(),
            TimePeriod = PerformanceTimePeriod
        };

        SortedDictionary<DateOnlyCustom, decimal> PortfolioReturns = PerformanceData.GetExtractedPropertyAsSeriesSorted(x => x.PortfolioReturn);
        SortedDictionary<DateOnlyCustom, decimal> BenchmarkReturns = PerformanceData.GetExtractedPropertyAsSeriesSorted(x => x.BenchmarkReturn);

        if(IsMoreThanAYear(PortfolioReturns.Keys.Min(), PortfolioReturns.Keys.Max()) == true)
        {
            results.AnnualizedBenchmarkReturn = StatisticsUtilities.CalculateAnnualizedReturn(BenchmarkReturns);
            results.AnnualizedPortfolioReturn = StatisticsUtilities.CalculateAnnualizedReturn(PortfolioReturns);
            results.AnnualizedExcessReturn = results.AnnualizedPortfolioReturn - results.AnnualizedBenchmarkReturn;
        }

        results.GainLoss = PerformanceData.Sum(x => x.Value.GainLoss);
        results.PortfolioReturn = StatisticsUtilities.CalculateTotalReturn(PortfolioReturns.Values.ToArray());
        results.BenchmarkReturn = StatisticsUtilities.CalculateTotalReturn(BenchmarkReturns.Values.ToArray());
        results.ExcessReturn = results.PortfolioReturn - results.BenchmarkReturn;

        try
        {
            results.Beta = StatisticsUtilities.CalculateBeta(PortfolioReturns.Values.ToArray(), BenchmarkReturns.Values.ToArray());
        }
        catch (DivideByZeroException) { }
        try
        {
            results.Correlation = StatisticsUtilities.CalculateCorrelation(PortfolioReturns.Values.ToArray(), BenchmarkReturns.Values.ToArray());
        }
        catch (DivideByZeroException) { }
        try
        {
            results.R_Squared = StatisticsUtilities.CalculateRSquared(PortfolioReturns.Values.ToArray(), BenchmarkReturns.Values.ToArray());
        }
        catch(DivideByZeroException) { }
        results.MaxDrawdown = StatisticsUtilities.CalculateMaxDrawdown(PortfolioReturns.Values.ToArray());
        results.AnnualizedVolatility = StatisticsUtilities.CalculateAnnulizedVolatility(PortfolioReturns);
        results.AnnualizedTrackingError = StatisticsUtilities.CalculateAnnulizedTrackingError(PortfolioReturns, BenchmarkReturns);
        return results;
    }

    /// <summary>
    /// Check if the period between StartDate and EndDate is more than a year.
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    private static bool IsMoreThanAYear(DateOnlyCustom StartDate, DateOnlyCustom EndDate)
    {
        TimeSpan timeSpan = EndDate - StartDate;
        return timeSpan.TotalDays > 365;
    }
}
