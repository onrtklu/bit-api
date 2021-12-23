using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBotApi.WebApi.Model
{
    public class ApiConfig
    {
        public string ApiUrlOrder { get; set; }
        public string ApiUrlCancel { get; set; }
        public string ApiUrlGetOrder { get; set; }
        public string ApiUrlGetOrderBook { get; set; }
    }
}