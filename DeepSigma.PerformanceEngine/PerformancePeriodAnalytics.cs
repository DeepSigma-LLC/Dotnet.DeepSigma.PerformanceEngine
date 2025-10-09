using DeepSigma.PerformanceEngine.Enums;
using DeepSigma.PerformanceEngine.Models;
using DeepSigma.Mathematics;
using DeepSigma.General.Extensions;

namespace DeepSigma.PerformanceEngine;

/// <summary>
/// Class to compute performance analytics from performance data points.
/// </summary>
internal class PerformancePeriodAnalytics
{
    private PerformanceTimePeriod PerformanceTimePeriod { get; set; }
    private SortedDictionary<DateTime, PerformanceDataPoint> PerformanceData { get; set; }
    internal PerformancePeriodAnalytics(SortedDictionary<DateTime, PerformanceDataPoint> performance_data, PerformanceTimePeriod time_period)
    {
        this.PerformanceData = performance_data;
        this.PerformanceTimePeriod = time_period;
    }

    /// <summary>
    /// Compute the performance analytics based on the performance data points.
    /// </summary>
    internal PerformanceAnalyticsResults GetComputePeformanceForPeriod()
    {
        PerformanceAnalyticsResults results = new();
        results.StartDate = PerformanceData.Keys.Min();
        results.EndDate = PerformanceData.Keys.Max();
        results.TimePeriod = PerformanceTimePeriod;

        SortedDictionary<DateTime, decimal> PortfolioReturns = PerformanceData.ToDictionary(x => x.Key, x => x.Value.PortfolioReturn).ToSortedDictionary();
        SortedDictionary<DateTime, decimal> BenchmarkReturns = PerformanceData.ToDictionary(x => x.Key, x => x.Value.BenchmarkReturn).ToSortedDictionary();

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

    private bool IsMoreThanAYear(DateTime StartDate, DateTime EndDate)
    {
        TimeSpan timeSpan = EndDate - StartDate;
        if (timeSpan.TotalDays < 365)
        {
            return false;
        }
        return true;
    }
}
