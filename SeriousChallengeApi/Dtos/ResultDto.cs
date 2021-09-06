using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousChallengeApi.Dtos
{

    public class ResultDto
    {
        [JsonProperty("c")]
        public float Price { get; set; }

        public DateTime InfoDate { get; set; }
    }
}
