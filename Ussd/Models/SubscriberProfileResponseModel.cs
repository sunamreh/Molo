using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class SubscriberProfileResponseModel
    {   
        [JsonPropertyName("subscriberId")]
        public Guid SubscriberId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }

        [JsonPropertyName("subscriptionDate")]
        public DateTimeOffset SubscriptionDate { get; set; }

        [JsonPropertyName("momoBalance")]
        public decimal MoMoBalance { get; set; }

        [JsonPropertyName("outstandingBalance")]
        public decimal OutstandingBalance { get; set; }

        [JsonPropertyName("activeClients")]
        public List<ActiveClientsDto> ActiveClients { get; set; }
    }

    public class ActiveClientsDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }
}
