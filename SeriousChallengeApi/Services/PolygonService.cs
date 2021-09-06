using Newtonsoft.Json;
using SeriousChallengeApi.Dtos;
using SeriousChallengeApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeriousChallengeApi.Services
{
    public class PolygonService
    {
        private readonly HttpClient _httpClient;

        private const string _apiKey = "OrgNNJNl7CkI7W5UOdurGAMp0Ec1JENP";

        public PolygonService(IHttpClientFactory httpClientFacotory)
        {
            _httpClient = httpClientFacotory.CreateClient("polygonHttpClient");
        }

        public async Task<StockSymbolDto> GetSymbolDatas(string symbolName,DateTime startDate, DateTime endDate)
        {
            var myURI = $"https://api.polygon.io/v2/aggs/ticker/{ symbolName }/range/1/day/{startDate.ToShortDateString()}/{endDate.ToShortDateString()}?apiKey={_apiKey}";

            var stockSymbol = new StockSymbolDto();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, myURI))
            {
                var response = await _httpClient.SendAsync(requestMessage);

                var content = await response.Content.ReadAsStringAsync();

                stockSymbol = JsonConvert.DeserializeObject<StockSymbolDto>(content);
            }

            return stockSymbol;
        }
    }
}
