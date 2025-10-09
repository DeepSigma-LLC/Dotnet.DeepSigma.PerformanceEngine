using DeepSigma.General;
using DeepSigma.PerformanceEngine.Enums;

namespace DeepSigma.PerformanceEngine.Models;

/// <summary>
/// Performance analytics results for a specified time period.
/// </summary>
public class PerformanceAnalyticsResults : IJSONSerializer<PerformanceAnalyticsResults>
{
    /// <summary>
    /// Time period for the performance analytics.
    /// </summary>
    public PerformanceTimePeriod TimePeriod { get; set; }
    /// <summary>
    /// Start date for the performance analytics.
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// End date for the performance analytics.
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// Gain or Loss amount for the period.
    /// </summary>
    public decimal GainLoss { get; set; }
    /// <summary>
    /// Return for the period.
    /// </summary>
    public decimal PortfolioReturn { get; set; }
    /// <summary>
    /// Benchmark Return for the period.
    /// </summary>
    public decimal BenchmarkReturn { get; set; }
    /// <summary>
    /// Excess Return for the period.
    /// </summary>
    public decimal ExcessReturn { get; set; }
    /// <summary>
    /// Annualized Portfolio Return for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal? AnnualizedPortfolioReturn { get; set; }
    /// <summary>
    /// Annualized Benchmark Return for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal? AnnualizedBenchmarkReturn { get; set; }
    /// <summary>
    /// Annualized Excess Return for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal? AnnualizedExcessReturn { get; set; }
    /// <summary>
    /// Annualized Tracking Error for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal AnnualizedTrackingError { get; set; }
    /// <summary>
    /// Beta for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal Beta { get; set; }
    /// <summary>
    /// Correlation for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal Correlation { get; set; }
    /// <summary>
    /// R-Squared for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal R_Squared { get; set; }
    /// <summary>
    /// Maximum Drawdown for the period.
    /// </summary>
    public decimal MaxDrawdown { get; set; }
    /// <summary>
    /// Annualized Volatility for the period if the period is more than a year; otherwise, null.
    /// </summary>
    public decimal AnnualizedVolatility { get; set; }

}
