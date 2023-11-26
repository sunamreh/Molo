using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Transfer.Models.Request
{
    public class PartyRequestModel
    {
        [JsonPropertyName("partyIdType")]
        public string PartyTypeId { get; set; }


        [JsonPropertyName("partyId")]
        public string PartyId { get; set; }
    }
}
