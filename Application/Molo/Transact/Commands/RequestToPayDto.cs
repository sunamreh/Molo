using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Transact.Commands
{
    public class RequestToPayDto
    {
        public string Amount { get; set; }

        public string Currency { get; set; }

        public Guid ExternalId { get; set; }

        public string PartyTypeId { get; set; }

        public string PartyId { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }
        public Guid TransactionId { get; set; }
    }
}
