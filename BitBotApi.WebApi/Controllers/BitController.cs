using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitBotApi.WebApi.Helper;
using BitBotApi.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BitBotApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitController : ControllerBase
    {
        private readonly BitConfig _bitConfig;
        private readonly ApiConfig _apiConfig;

        public BitController(IOptions<BitConfig> bitconfig, IOptions<ApiConfig> apiconfig)
        {
            _bitConfig = bitconfig.Value;
            _apiConfig = apiconfig.Value;
        }
        
        /// <summary>
        /// Satış emri için gerekli olan header ve body verilerini döner
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "volume": 15.00, // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı,
        ///        "price": 49.00 // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///     }
        ///
        /// </remarks>
        [HttpGet("sell-header-body")]
        public IActionResult GetHeaderAndBodySell(string volume, string marketCode, string price)
        {
            var requestBody = GetSellRequestBody(volume, marketCode, price);

            var result = GetHeaderAndBodySring(requestBody);

            return Ok(result);
        }
        
        /// <summary>
        /// Alış emri için gerekli olan header ve body verilerini döner
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "volume": 15.00, // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı,
        ///        "price": 49.00 // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///     }
        ///
        /// </remarks>
        [HttpGet("buy-header-body")]
        public IActionResult GetHeaderAndBodyBuy(string volume, string marketCode, string price)
        {
            var requestBody = GetBuyRequestBody(volume, marketCode, price);

            var result = GetHeaderAndBodySring(requestBody);

            return Ok(result);
        }

        /// <summary>
        /// Satış emri için istek gönderir
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "volume": 15.00, // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı,
        ///        "price": 49.00 // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///     }
        ///
        /// </remarks>
        [HttpGet("sell-send-request")]
        public async Task<IActionResult> SendSellRequest(string volume, string marketCode, string price)
        {
            var requestBody = GetSellRequestBody(volume, marketCode, price);

            var result = await SendRequst(requestBody, _apiConfig.ApiUrlOrder);
            
            return Ok(result);
        }

        /// <summary>
        /// Alış emri için istek gönderir
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "volume": 15.00, // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı,
        ///        "price": 49.00 // gibi olmalıdır. Noktadan sonra 2 basamak eklenmeli
        ///     }
        ///
        /// </remarks>
        [HttpGet("buy-send-request")]
        public async Task<IActionResult> SendBuyRequest(string volume, string marketCode, string price)
        {
            var requestBody = GetBuyRequestBody(volume, marketCode, price);

            var result = await SendRequst(requestBody, _apiConfig.ApiUrlOrder);
            
            return Ok(result);
        }

        /// <summary>
        /// Emri iptal eder
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "id": 42323 // alış ya da satış isteğinin sonucunda dönen id değeri girilmeli
        ///     }
        ///
        /// </remarks>
        [HttpGet("cancel-order/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await SendRequst("{}", string.Format(_apiConfig.ApiUrlCancel, id));
            
            return Ok(result);
        }

        /// <summary>
        /// Alış-Satış emir durumunu gösterir
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı
        ///     }
        ///
        /// </remarks>
        [HttpGet("orders/{marketCode}")]
        public async Task<IActionResult> GetOrder(string marketCode)
        {
            var result = await SendRequstGet("{}", string.Format(_apiConfig.ApiUrlGetOrder, marketCode));
            
            return Ok(result);
        }

        /// <summary>
        /// Girilen market koduna göre son işlem yapılan fiyat, en yakın alış isteği ve en yakın satış isteği gösterilir
        /// </summary>
        /// <remarks>
        /// Daha ayrıntılı bilgi için <a href="https://www.bitexen.com/api/v1/order_book/VANTRY/" target="_blank">Bu sayfadan bakabilirsiniz</a>
        /// 
        /// Sample request:
        ///
        ///     GET 
        ///     {
        ///        "marketCode": "VANTRY", // gibi olmalıdır. Büyük harf ve takım token kodu önce yazılmalı
        ///     }
        ///
        /// </remarks>
        [HttpGet("order_book/{marketCode}")]
        public async Task<IActionResult> GetOrderBook(string marketCode)
        {
            var jsonResult = await SendRequstGet("{}", string.Format(_apiConfig.ApiUrlGetOrderBook, marketCode));

            var orderBook = JsonConvert.DeserializeObject<OrderBookModel>(jsonResult);

            var result = new OrderBookModelDto()
            {
                LastTransactionPrice = orderBook?.Data?.LastTransactions?.FirstOrDefault()?.Price,
                MaxBuyerPrice = orderBook?.Data?.Buyers?.FirstOrDefault()?.OrdersPrice,
                MinSellerPrice = orderBook?.Data?.Sellers?.FirstOrDefault()?.OrdersPrice
            };
            
            return Ok(result);
        }
        
        #region Methods

        #region RequestBody
        private string GetSellRequestBody(string volume, string marketCode, string price)
        {
            var model = GetRequestModel(volume, marketCode, price, "S");

            var result = JsonConvert.SerializeObject(model);
            return result;
        }

        private string GetBuyRequestBody(string volume, string marketCode, string price)
        {
            var model = GetRequestModel(volume, marketCode, price, "B");

            var result = JsonConvert.SerializeObject(model);
            return result;
        }

        private BitRequestModel GetRequestModel(string volume, string marketCode, string price,  string buySel)
        {
            var model = new BitRequestModel()
            {
                Volume = volume,
                MarketCode = marketCode,
                BuySell = buySel,
                AccountNumber = _bitConfig.AccountNumber,
                OrderType = "limit",
                Price = price
            };

            return model;
        }

        #endregion

        #region Request
        
        private async Task<string> SendRequstGet(string requestBody, string url)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            SetHeader(request, requestBody);
            request.AddParameter("text/plain", requestBody,  ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return response.Content;
        }

        private async Task<string> SendRequst(string requestBody, string url)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            SetHeader(request, requestBody);
            request.AddParameter("text/plain", requestBody,  ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return response.Content;
        }

        private void SetHeader(RestRequest request, string requestBody)
        {
            var headers = GetHeaders(requestBody);
            
            foreach (var item in headers)
            {
                request.AddHeader(item.Key, item.Value);
            }
        }

        private Dictionary<string,string> GetHeaders(string requestBody)
        {
            var result = new Dictionary<string,string>();

            var now = DateTime.Now;
            var timeStamp = ((DateTimeOffset)now).ToUnixTimeSeconds().ToString();

            var signature = BitHelper.StringEncode(_bitConfig.ApiKey + _bitConfig.AccessUser + _bitConfig.PassPhrase + timeStamp + requestBody);
            var sKey = BitHelper.StringEncode(_bitConfig.SecretKey);
            var hash = new HMACSHA256(sKey);
            var accessSign = BitHelper.HashEncode(hash.ComputeHash(signature)).ToUpper();

            result.Add("cache-control", "no-cache");
            result.Add("access-user", _bitConfig.AccessUser);
            result.Add("access-passphrase", _bitConfig.PassPhrase);
            result.Add("access-key", _bitConfig.ApiKey);
            result.Add("access-timestamp", timeStamp);
            result.Add("access-sign", accessSign);
            result.Add("Content-Type", "text/plain");

            return result;
        }

        #endregion
        
        #region Write Header and Body

        private string GetHeaderAndBodySring(string requestBody)
        {
            StringBuilder sb = new StringBuilder();

            var headerStr = GetHeaderString(requestBody);

            sb.AppendLine(headerStr);

            sb.AppendLine("");
            sb.AppendLine("Body: ");
            sb.AppendLine(requestBody);

            return sb.ToString();
        }

        private string GetHeaderString(string requestBody)
        {
            StringBuilder sb = new StringBuilder();

            var headers = GetHeaders(requestBody);

            sb.AppendLine("Headers: ");
            foreach (var item in headers)
            {
                sb.AppendLine(item.Key + ": " + item.Value);
            }

            return sb.ToString();
        }

        #endregion

        #endregion

    }
}