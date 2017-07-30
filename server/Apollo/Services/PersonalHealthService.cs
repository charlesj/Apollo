using System;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;

namespace Apollo.Services
{
    public interface IPersonalHealthService
    {
        Task<decimal> CalculateRecentlyLostWeight();
        Task<decimal> TotalWeightChange();
    }

    public class PersonalHealthService : IPersonalHealthService
    {
        private readonly IMetricsDataService metricsDataService;

        public PersonalHealthService(IMetricsDataService metricsDataService)
        {
            this.metricsDataService = metricsDataService;
        }

        public async Task<decimal> CalculateRecentlyLostWeight()
        {
            var weightMetrics = (await metricsDataService.GetMetrics(null, "weight")).Reverse().Take(7).ToList();
            if (!weightMetrics.Any())
                return 0;
            return weightMetrics.First().value - weightMetrics.Last().value;
        }

        public async Task<decimal> TotalWeightChange()
        {
            var weightMetrics = await metricsDataService.GetMetrics(null, "weight");
            var maxWeight = weightMetrics.Max(m => m.value);
            var minWeight = weightMetrics.Min(m => m.value);
            return minWeight - maxWeight;
        }
    }
}
