using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Response
{
    public class PartyResponseModel
    {
        [JsonPropertyName("partyIdType")]
        public string PartyTypeId { get; set; }


        [JsonPropertyName("partyId")]
        public string PartyId { get; set; }
    }
}
