using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class CollectBalanceRequestModel
    {
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }
    }
}
