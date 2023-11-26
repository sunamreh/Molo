namespace Molo.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public Guid SubscriberClientId { get; set; }
        public virtual Client SubscriberClient { get; set; }
        public Guid SubscriberLoanId { get; set; }
        public virtual Loan SubscriberLoan { get; set; }
        public byte TransactionTypeId { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public Guid ExternalId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
