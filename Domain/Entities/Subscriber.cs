namespace Molo.Domain.Entities
{
    public class Subscriber : BaseEntity
    {
        public string Name { get; set; }
        public string Msisdn { get; set; }
        public string Pin { get; set; }
        public DateTimeOffset SubscriptionDate { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? DateInactive { get; set; }
        public virtual IList<Client> Clients { get; set; }
        public virtual IList<Loan> Loans { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
    }
}
