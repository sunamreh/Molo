namespace Molo.Domain.Entities
{
    public class Loan : BaseEntity
    {
        public Guid SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public Guid SubscriberClientId { get; set; }
        public virtual Client SubscriberClient { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset CollectionDate { get; set; }
        public bool IsSettled { get; set; }
        public DateTimeOffset? SettlementDate { get; set; }
        public byte InterestRateId { get; set; }
        public virtual InterestRate InterestRate { get; set; }
    }
}
