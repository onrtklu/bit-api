using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace BitBotApi.WebApi.Model
{
    public class BitRequestModel
    {
        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("market_code")]
        public string MarketCode { get; set; }

        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        [JsonProperty("account_number")]
        public int AccountNumber { get; set; }

        [JsonProperty("order_type")]
        public string OrderType { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}