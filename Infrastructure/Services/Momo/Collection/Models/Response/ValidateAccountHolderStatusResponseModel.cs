using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Response
{
    public class ValidateAccountHolderStatusResponseModel
    {
        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }
}
