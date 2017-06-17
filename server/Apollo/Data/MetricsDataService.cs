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
    
    public class MetricsDataService : IMetricsDataService
    {
        public const string InsertSql = "insert into metrics (category, name, value, created_at)" +
                                        " values (@category, @name, @value, current_timestamp)";

        public const string NameSelectSql = "select * from metrics where name=@name";
        
        public const string CategorySelectSql = "select * from metrics where category=@category";
        
        public const string AllSelectSql = "select * from metrics";

        public const string BothSelectSql = "select * from metrics where name=@name and category=@category";
        
        private readonly IDbConnectionFactory dbConnectionFactory;

        public MetricsDataService(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }
        
        public async Task InsertMetric(string category, string name, float value)
        {
            using (var connection = await this.dbConnectionFactory.GetConnection())
            {
                connection.Execute(InsertSql, new {category, name, value});
            }
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

            using (var connection = await this.dbConnectionFactory.GetConnection())
            {
                return (await connection.QueryAsync<Metric>(sql, new {category, name})).ToList();
            }
        }
    }
    
    public class Metric
    {
        public int id { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public DateTimeOffset created_at { get; set; }
    }
}