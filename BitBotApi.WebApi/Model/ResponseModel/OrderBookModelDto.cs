using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBotApi.WebApi.Model
{
    public class OrderBookModelDto
    {
        public string LastTransactionPrice { get; set; }
        public string MaxBuyerPrice { get; set; }
        public string MinSellerPrice { get; set; }
    }
}