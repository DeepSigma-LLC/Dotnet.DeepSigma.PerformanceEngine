
using DeepSigma.General;

namespace DeepSigma.PerformanceEngine.Models
{
    /// <summary>
    /// Summary statistics for a set of performance data points.
    /// </summary>
    public class SummaryStatistics : IJSONIO<SummaryStatistics>
    {
        /// <summary>
        /// The latest date in the observation period.
        /// </summary>
        public DateTime MaxDate { get; set; }
        /// <summary>
        /// The earliest date in the observation period.
        /// </summary>
        public DateTime MinDate { get; set; }
        /// <summary>
        /// Total days in the observation period.
        /// </summary>
        public double TotalDaysInObservationPeriod { get; set; }
        /// <summary>
        /// Total business days in the observation period.
        /// </summary>
        public double TotalBusinessDaysInObservationPeriod { get; set; }
        /// <summary>
        /// Total observations in the observation period.
        /// </summary>
        public int ObservationCount { get; set; }
        /// <summary>
        /// The last recorded value in the observation period.
        /// </summary>
        public decimal LastRecordedValue { get; set; }
        /// <summary>
        /// The first recorded value in the observation period.
        /// </summary>
        public decimal EarliestRecordedValue { get; set; }
        /// <summary>
        /// Mean of the observations in the observation period.
        /// </summary>
        public decimal Mean { get; set; }
        /// <summary>
        /// Median of the observations in the observation period.
        /// </summary>
        public decimal Median { get; set; }
        /// <summary>
        /// Minimum value in the observation period.
        /// </summary>
        public decimal Min { get; set; }
        /// <summary>
        /// Maximum value in the observation period.
        /// </summary>
        public decimal Max { get; set; }
        /// <summary>
        /// Annualized Return of the observations in the observation period.
        /// </summary>
        public decimal AnnualizedReturn { get; set; }
        /// <summary>
        /// Cumulative Return of the observations in the observation period.
        /// </summary>
        public decimal CumulativeReturn { get; set; }
        /// <summary>
        /// Standard Deviation of the observations in the observation period.
        /// </summary>
        public decimal StandardDeviation { get; set; }
        /// <summary>
        /// Variance of the observations in the observation period.
        /// </summary>
        public decimal MaxDrawdownPercentage { get; set; }
        /// <summary>
        /// Sharpe Ratio of the observations in the observation period.
        /// </summary>
        public decimal SharpeRatio { get; set; }
        /// <summary>
        /// Information Ratio of the observations in the observation period.
        /// </summary>
        public decimal InformationRatio { get; set; }
        /// <summary>
        /// Tracking Error of the observations in the observation period.
        /// </summary>
        public decimal TrackingError { get; set; }
        /// <summary>
        /// Up Capture Ratio of the observations in the observation period.
        /// </summary>
        public decimal UpCaptureRatio { get; set; }
        /// <summary>
        /// Down Capture Ratio of the observations in the observation period.
        /// </summary>
        public decimal DownCaptureRatio { get; set; }
        /// <summary>
        /// Alpha of the observations in the observation period.
        /// </summary>
        public decimal Alpha { get; set; }
        /// <summary>
        /// Beta of the observations in the observation period.
        /// </summary>
        public decimal Beta { get; set; }
        /// <summary>
        /// R-Squared of the observations in the observation period.
        /// </summary>
        public decimal Correlation { get; set; }
        /// <summary>
        /// R-Squared of the observations in the observation period.
        /// </summary>
        public decimal RSquared { get; set; }
        /// <summary>
        /// Sortino Ratio of the observations in the observation period.
        /// </summary>
        public decimal SortinoRatio { get; set; }
        /// <summary>
        /// Skewness of the observations in the observation period.
        /// </summary>
        public decimal Skewness { get; set; }
        /// <summary>
        /// Kurtosis of the observations in the observation period.
        /// </summary>
        public decimal Kurtosis { get; set; }
        /// <summary>
        /// Batting Average of the observations in the observation period.
        /// </summary>
        public decimal BattingAverage { get; set; }
    }
}
