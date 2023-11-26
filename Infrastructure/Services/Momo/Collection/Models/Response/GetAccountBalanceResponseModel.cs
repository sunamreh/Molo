using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Response
{
    public class GetAccountBalanceResponseModel
    {
        [JsonPropertyName("availableBalance")]
        public string AvailableBalance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
