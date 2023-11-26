using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Response
{
    public class RequesttoPayTransactionStatusResponseModel
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("financialTransactionId")]
        public string FinancialTransactionId { get; set; }

        [JsonPropertyName("externalId")]
        public string ExternalId { get; set; }

        [JsonPropertyName("payer")]
        public PartyResponseModel Party { get; set; }

        [JsonPropertyName("payerMessage")]
        public string PayerMessage { get; set; }

        [JsonPropertyName("payeeNote")]
        public string PayeeNote { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("reason")]
        public ErrorResponseModel Reason { get; set; }
    }
}
