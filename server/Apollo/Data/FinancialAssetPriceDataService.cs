using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IFinancialAssetPriceDataService
    {
        Task RecordPrice(string symbol, decimal price, string source, DateTimeOffset validAt);
        Task<FinancialAssetPrice> GetMostRecentPrice(string identifier);
    }

    public class FinancialAssetPriceDataService : BaseDataService, IFinancialAssetPriceDataService
    {
        public const string RecordPriceSql =
            "insert into financial_asset_prices(symbol,price,source,created_at,valid_at) " +
            "values (@symbol, @price, @source, current_timestamp, @validAt)";

        public const string GetMostRecentPriceSql = "select * from financial_asset_prices " +
                                                    "where symbol=@symbol " +
                                                    "order by valid_at desc limit 1";

        public FinancialAssetPriceDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public Task RecordPrice(string symbol, decimal price, string source, DateTimeOffset validAt)
        {
            return Execute(RecordPriceSql, new {symbol, price, source, validAt});
        }

        public async Task<FinancialAssetPrice> GetMostRecentPrice(string identifier)
        {
            var results = await QueryAsync<FinancialAssetPrice>(GetMostRecentPriceSql, new {identifier});
            return results.SingleOrDefault();
        }
    }

    public class FinancialAssetPrice
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public decimal price { get; set; }
        public string source { get; set; }
        public DateTime created_at { get; set; }
        public DateTime valid_at { get; set; }
    }
}
