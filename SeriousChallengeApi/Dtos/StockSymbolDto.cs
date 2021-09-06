using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousChallengeApi.Dtos
{
    public class StockSymbolDto
    {
        [JsonProperty("ticker")]
        public string Name { get; set; }

        public int WeekDay { get; set; }

        public List<ResultDto> Results { get; set; }

        [JsonProperty("resultsCount")]
        public int ResultsCount { get; set; }
    }
}
