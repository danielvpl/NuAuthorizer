using Newtonsoft.Json;

namespace Domain.Aggregates
{
    public class Account : Entity
    {
        [JsonProperty("active-card")]
        public bool ActiveCard { get; set; }
        [JsonProperty("available-limit")]
        public int AvailableLimit { get; set; }

        public Account() { }

        public Account(bool ActiveCard, int AvailableLimit)
        {
            this.ActiveCard = ActiveCard;
            this.AvailableLimit = AvailableLimit;
        }        
    }
}
