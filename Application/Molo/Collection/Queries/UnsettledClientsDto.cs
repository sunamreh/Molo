namespace Molo.Application.Molo.Collection.Queries
{
    public class UnsettledClientsDto
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Msisdn { get; set; }
        public decimal Balance { get; set; }
    }
}
