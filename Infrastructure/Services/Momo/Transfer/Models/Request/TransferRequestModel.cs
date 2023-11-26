using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Transfer.Models.Request
{
    public class TransferRequestModel
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }


        [JsonPropertyName("currency")]
        public string Currency { get; set; }


        [JsonPropertyName("externalId")]
        public string ExternalId { get; set; }


        [JsonPropertyName("payee")]
        public PartyRequestModel Payee { get; set; }


        [JsonPropertyName("payerMessage")]
        public string PayerMessage { get; set; }


        [JsonPropertyName("payeeNote")]
        public string PayeeNote { get; set; }
    }
}
