using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBotApi.WebApi.Model
{
    public class BitConfig
    {
        public string AccessUser { get; set; } 
        public string PassPhrase { get; set; } 
        public string ApiKey { get; set; } 
        public int AccountNumber { get; set; } 
        public string SecretKey { get; set; } 
    }
}