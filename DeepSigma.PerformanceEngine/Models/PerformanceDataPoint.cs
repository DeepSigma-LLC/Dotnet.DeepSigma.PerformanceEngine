

namespace DeepSigma.PerformanceEngine.Models;

/// <summary>
/// Data point for performance calculations
/// </summary>
public class PerformanceDataPoint()
{
    /// <summary>
    /// Portfolio Identifier.
    /// </summary>
    public int PortfolioId { get; set; }

    /// <summary>
    /// Date of the data point.
    /// </summary>
    public DateTime DataDate { get; set; }

    /// <summary>
    /// Gain or Loss amount for the period.
    /// </summary>
    public decimal GainLoss { get; set; }

    /// <summary>
    /// Denominator for the start of the period.
    /// </summary>
    public decimal Denominator { get; set; }

    /// <summary>
    /// Return for the period.
    /// </summary>
    public decimal PortfolioReturn { get; set; }

    /// <summary>
    /// Benchmark Return for the period.
    /// </summary>
    public decimal BenchmarkReturn { get; set; }

    /// <summary>
    /// Risk Free Rate for the period.
    /// </summary>
    public decimal RiskFreeRate { get; set; }
}
