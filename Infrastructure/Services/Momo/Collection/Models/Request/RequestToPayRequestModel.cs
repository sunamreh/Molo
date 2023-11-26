using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Request
{
    public class RequestToPayRequestModel
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }


        [JsonPropertyName("currency")]
        public string Currency { get; set; }


        [JsonPropertyName("externalId")]
        public string ExternalId { get; set; }


        [JsonPropertyName("payer")]
        public PartyRequestModel Payer { get; set; }


        [JsonPropertyName("payerMessage")]
        public string PayerMessage { get; set; }


        [JsonPropertyName("payeeNote")]
        public string PayeeNote { get; set; }
    }
}
