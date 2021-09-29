using Domain.Aggregates;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.Responses
{
    public class OperationResult
    {
        [JsonProperty("account")]
        public Account Account { get; set; }
        [JsonProperty("violations")]
        public List<string> Violations { get; set; }             
    }
}
