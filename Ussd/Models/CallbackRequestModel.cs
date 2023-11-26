using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ussd.Models
{
    public class CallbackRequestModel
    {
        [JsonIgnore]
        [FromForm(Name = "sessionId")]
        public string? SessionId { get; set; }

        [JsonIgnore]
        [FromForm(Name = "phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonIgnore]
        [FromForm(Name = "networkCode")]
        public string? NetworkCode { get; set; }

        [JsonIgnore]
        [FromForm(Name = "serviceCode")]
        public string? ServiceCode { get; set; }

        [JsonIgnore]
        [FromForm(Name = "text")]
        public string? Text { get; set; }
    }
}
