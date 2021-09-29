using Newtonsoft.Json;
using System;

namespace Domain.Aggregates
{
    public class Transaction : Entity
    {
        [JsonProperty("merchant")]
        public string Merchant { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
