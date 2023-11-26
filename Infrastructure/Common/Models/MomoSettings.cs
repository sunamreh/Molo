namespace Molo.Infrastructure.Common.Models
{
    public class MomoSettings
    {
        public string HostUrl { get; set; }
        public Collection Collection { get; set; }
        public Disbursement Disbursement { get; set; }
    }

    public class Disbursement
    {
        public OAuth2 OAuth2 { get; set; }
        public Transfer Transfer { get; set; }
    }

    public class Collection
    {
        public OAuth2 OAuth2 { get; set; }
        public RequestToPay RequestToPay { get; set; }
        public GetAccountBalance GetAccountBalance { get; set; }
        public ValidateAccountHolderStatus ValidateAccountHolderStatus { get; set; }
        public RequesttoPayDeliveryNotification RequesttoPayDeliveryNotification { get; set; }
    }

    public class OAuth2
    {
        public string Path { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string XTargetEnvironment { get; set; }
        public string ContentType { get; set; }
        public string CacheControl { get; set; }
        public string OcpApimSubscriptionKey { get; set; }
        public string GrantType { get; set; }
        public int TokenExpiryThresholdPercentage { get; set; }
    }

    public class RequestToPay
    {
        public string Path { get; set; }
        public string CallbackUrl { get; set; }
    }

    public class GetAccountBalance
    {
        public string Path { get; set; }
    }

    public class ValidateAccountHolderStatus
    {
        public string Path { get; set; }
    }

    public class RequesttoPayDeliveryNotification
    {
        public string Path { get; set; }
    }

    public class Transfer
    {
        public string Path { get; set; }
    }
}
