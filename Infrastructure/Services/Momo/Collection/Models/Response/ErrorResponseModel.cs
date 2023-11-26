using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Response
{
    public class ErrorResponseModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
