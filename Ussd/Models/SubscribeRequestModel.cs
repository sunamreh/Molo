using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class SubscribeRequestModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }

        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
}
