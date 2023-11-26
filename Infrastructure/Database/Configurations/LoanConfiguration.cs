using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;

namespace Molo.Infrastructure.Database.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                .HasPrecision(18,2)
                .IsRequired();

            builder.Property(t => t.CollectionDate)
                .IsRequired();

            builder.Property(t => t.Currency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(t => t.InterestRateId)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(t => t.IsSettled)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(t => t.SubscriberClientId)
                .IsRequired();

            builder.Property(t => t.SubscriberId)
                .IsRequired();

            builder.HasOne(t => t.Subscriber);

            builder.HasOne(t => t.SubscriberClient);

            builder.HasOne(t => t.InterestRate);
        }
    }
}
