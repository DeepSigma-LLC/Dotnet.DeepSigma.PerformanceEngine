using DeepSigma.General.Extensions;
using DeepSigma.PerformanceEngine.Enums;
using DeepSigma.General;
using DeepSigma.PerformanceEngine.Models;
using DeepSigma.General.Enums;

namespace DeepSigma.PerformanceEngine
{
    /// <summary>
    /// Engine to calculate performance analytics from performance data points.
    /// </summary>
    public class PerformanceEngine
    {
        private SortedDictionary<DateTime, PerformanceDataPoint> PerformanceData { get; set; } = [];
        private SortedDictionary<DateTime, decimal> PortfolioReturns { get; set; } = new SortedDictionary<DateTime, decimal>();
        private SortedDictionary<DateTime, decimal> BenchmarkReturns { get; set; } = new SortedDictionary<DateTime, decimal>();

        /// <summary>
        /// Initialize the performance engine with performance data points.
        /// </summary>
        /// <param name="performance_data"></param>
        public PerformanceEngine(SortedDictionary<DateTime, PerformanceDataPoint> performance_data)
        {
            this.PerformanceData = performance_data;
            this.PortfolioReturns = PerformanceData.ToDictionary(x => x.Key, x => x.Value.PortfolioReturn).ToSortedDictionary();
            this.BenchmarkReturns = PerformanceData.ToDictionary(x => x.Key, x => x.Value.BenchmarkReturn).ToSortedDictionary();
        }

        /// <summary>
        /// Calculate performance summary for standard time periods.
        /// </summary>
        /// <param name="as_of_date"></param>
        /// <returns></returns>
        public List<PerformanceAnalyticsResults> CalculatePerformanceDataSummary(DateTime as_of_date)
        {
            List<PerformanceAnalyticsResults> results = [];
            foreach (PerformanceTimePeriod period in Enum.GetValues(typeof(PerformanceTimePeriod)))
            {
                DateTime startDate = GetPeriodStartDate(PortfolioReturns.Keys.Min(), PortfolioReturns.Keys.Max(), period);
                DateTime endDate = as_of_date;
                if(PortfolioReturns.Keys.Min() <= startDate)
                {
                    SortedDictionary<DateTime, PerformanceDataPoint> selected_data = this.PerformanceData.Where(x => x.Key <= endDate && x.Key >= startDate).ToSortedDictionary();
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
        public List<PerformanceAnalyticsResults> CalculatePerformanceForCustomPeriodicity(DateTime as_of_date, Periodicity periodicity, PerformanceTimePeriod performance_time_period)
        {
            List<PerformanceAnalyticsResults> results = [];
            SelfAligningTimeStep time_step = new(periodicity, TimeInterval.Min_5, false);
            DateTime min_date = PortfolioReturns.Keys.Min();
            DateTime selected_data_time = as_of_date;
            while (selected_data_time >= min_date)
            {
                DateTime TargetEndDate = selected_data_time;
                DateTime TargetStartDate = time_step.GetPreviousTimeStep(selected_data_time).AddDays(1);
                SortedDictionary<DateTime, PerformanceDataPoint> selected_data = this.PerformanceData.Where(x => x.Key <= TargetEndDate && x.Key >= TargetStartDate).ToSortedDictionary();
                PerformancePeriodAnalytics item = new(selected_data, performance_time_period);
                
                var result = item.GetComputePeformanceForPeriod();
                results.Add(result);
                selected_data_time = TargetStartDate.AddDays(-1);
            }
            return results;
        }

        private DateTime GetPeriodStartDate(DateTime start_date, DateTime end_date, PerformanceTimePeriod period)
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
                    SelfAligningTimeStep weeklyTimeStep = new(Periodicity.Weekly, TimeInterval.Min_5, false);
                    return weeklyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
                case (PerformanceTimePeriod.MTD):
                    SelfAligningTimeStep monthlyTimeStep = new(Periodicity.Monthly, TimeInterval.Min_5, false);
                    return monthlyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
                case (PerformanceTimePeriod.QTD):
                    SelfAligningTimeStep quarterlyTimeStep = new(Periodicity.Quarterly, TimeInterval.Min_5, false);
                    return quarterlyTimeStep.GetPreviousTimeStep(end_date).AddDays(1);
                case (PerformanceTimePeriod.YTD):
                    SelfAligningTimeStep yearlyTimeStep = new(Periodicity.Annually, TimeInterval.Min_5, false);
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
}
