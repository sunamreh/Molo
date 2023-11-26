using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;

namespace Molo.Infrastructure.Database.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.SubscriberClientId)
                .IsRequired();

            builder.Property(t => t.SubscriberId)
                .IsRequired();

            builder.Property(t => t.SubscriberLoanId)
                .IsRequired();

            builder.Property(t => t.TransactionAmount)
                .HasPrecision(18,2)
                .IsRequired();

            builder.Property(t => t.TransactionTypeId)
                .IsRequired();

            builder.Property(t => t.ExternalId)
                .IsRequired();

            builder.Property(t => t.TransactionId)
                .IsRequired();

            builder.HasOne(t => t.Subscriber);

            builder.HasOne(t => t.SubscriberClient);

            builder.HasOne(t => t.SubscriberLoan);

            builder.HasOne(t => t.TransactionType);
        }
    }
}
