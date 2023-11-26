namespace Molo.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Guid SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public string Name { get; set; }
        public string Msisdn { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public bool IsActive { get; set; }
        public virtual IList<Loan> Loans { get; set; }
    }
}
