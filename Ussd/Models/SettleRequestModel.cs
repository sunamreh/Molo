using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class SettleRequestModel
    {
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }
    }
}
