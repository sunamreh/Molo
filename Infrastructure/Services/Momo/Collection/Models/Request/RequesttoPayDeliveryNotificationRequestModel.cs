using System.Text.Json.Serialization;

namespace Molo.Infrastructure.Services.Momo.Collection.Models.Request
{
    public class RequesttoPayDeliveryNotificationRequestModel
    {
        [JsonPropertyName("notificationMessage")]
        public string NotificationMessage { get; set; }
    }
}
