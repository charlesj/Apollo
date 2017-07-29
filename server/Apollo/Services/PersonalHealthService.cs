using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;

namespace Apollo.Services
{
    public interface IPersonalHealthService
    {
        Task<decimal> CalculateRecentlyLostWeight();
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
    }
}
