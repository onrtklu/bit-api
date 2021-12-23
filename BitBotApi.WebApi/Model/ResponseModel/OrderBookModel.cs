using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BitBotApi.WebApi.Model
{
    public class OrderBookModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Market
    {
        [JsonProperty("market_code")]
        public string MarketCode { get; set; }

        [JsonProperty("base_currency_code")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("counter_currency_code")]
        public string CounterCurrencyCode { get; set; }
    }

    public class Ticker
    {
        [JsonProperty("market")]
        public Market Market { get; set; }

        [JsonProperty("bid")]
        public string Bid { get; set; }

        [JsonProperty("ask")]
        public string Ask { get; set; }

        [JsonProperty("last_price")]
        public string LastPrice { get; set; }

        [JsonProperty("last_size")]
        public string LastSize { get; set; }

        [JsonProperty("volume_24h")]
        public string Volume24h { get; set; }

        [JsonProperty("change_24h")]
        public string Change24h { get; set; }

        [JsonProperty("low_24h")]
        public string Low24h { get; set; }

        [JsonProperty("high_24h")]
        public string High24h { get; set; }

        [JsonProperty("avg_24h")]
        public string Avg24h { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public class Buyer
    {
        [JsonProperty("orders_total_amount")]
        public string OrdersTotalAmount { get; set; }

        [JsonProperty("orders_price")]
        public string OrdersPrice { get; set; }
    }

    public class Seller
    {
        [JsonProperty("orders_total_amount")]
        public string OrdersTotalAmount { get; set; }

        [JsonProperty("orders_price")]
        public string OrdersPrice { get; set; }
    }

    public class LastTransaction
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Data
    {
        [JsonProperty("market_code")]
        public string MarketCode { get; set; }

        [JsonProperty("ticker")]
        public Ticker Ticker { get; set; }

        [JsonProperty("buyers")]
        public List<Buyer> Buyers { get; set; }

        [JsonProperty("sellers")]
        public List<Seller> Sellers { get; set; }

        [JsonProperty("last_transactions")]
        public List<LastTransaction> LastTransactions { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}