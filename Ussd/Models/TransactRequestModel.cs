using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class TransactRequestModel
    {
        [JsonPropertyName("subscriberId")]
        public Guid SubscriberId { get; set; }

        [JsonPropertyName("clientName")]
        public string ClientName { get; set; }

        [JsonPropertyName("clientMsisdn")]
        public string ClientMsisdn { get; set; }

        [JsonPropertyName("loanAmount")]
        public string LoanAmount { get; set; }

        [JsonPropertyName("collectionDate")]
        public string CollectionDate { get; set; }

        [JsonPropertyName("interestRateId")]
        public byte InterestRateId { get; set; }
    }
}
