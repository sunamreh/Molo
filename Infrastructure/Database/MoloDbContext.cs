using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Molo.Domain.Entities;
using Molo.Infrastructure.Common.Helpers;
using System.Reflection;
using System.Reflection.Emit;

namespace Molo.Infrastructure.Database
{
    public class MoloDbContext : DbContext
    {
        public MoloDbContext(DbContextOptions<MoloDbContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetConverter>();
        }

        public DbSet<InterestRate> InterestRate { get; set; }
        public DbSet<Subscriber> Subscriber { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
    }
}
