using Domain.Aggregates;
using Newtonsoft.Json;

namespace Domain.Commands.Requests
{
    public class AuthorizeTransactionCommand
    {
        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }
        public Account Account { get; set; }
    }
}