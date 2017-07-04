using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IMetricsDataService
    {
        Task InsertMetric(string category, string name, float value);
        Task<IReadOnlyList<Metric>> GetMetrics(string category, string name);
    }

    public class MetricsDataService : BaseDataService, IMetricsDataService
    {
        public const string InsertSql = "insert into metrics (category, name, value, created_at)" +
                                        " values (@category, @name, @value, current_timestamp)";

        public const string NameSelectSql = "select * from metrics where name=@name order by id asc";

        public const string CategorySelectSql = "select * from metrics where category=@category order by id asc";

        public const string AllSelectSql = "select * from metrics order by id asc";

        public const string BothSelectSql = "select * from metrics where name=@name and category=@category order by id asc";

        public MetricsDataService(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task InsertMetric(string category, string name, float value)
        {
            await Execute(InsertSql, new {category, name, value});
        }

        public async Task<IReadOnlyList<Metric>> GetMetrics(string category, string name)
        {
            var sql = AllSelectSql;
            if (string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(name))
                sql = NameSelectSql;
            else if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(category))
                sql = CategorySelectSql;
            else if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(category))
                sql = BothSelectSql;

            return await QueryAsync<Metric>(sql, new {category, name});
        }
    }

    public class Metric
    {
        public int id { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public DateTime created_at { get; set; }
    }
}
