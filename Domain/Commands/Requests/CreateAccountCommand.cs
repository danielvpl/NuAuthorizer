using Domain.Aggregates;
using Newtonsoft.Json;

namespace Domain.Commands.Requests
{
    public class CreateAccountCommand
    {
        [JsonProperty("account")]
        public Account Account { get; set; }
    }
}