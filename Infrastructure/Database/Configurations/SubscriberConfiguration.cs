using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;

namespace Molo.Infrastructure.Database.Configurations
{
    public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Pin)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(t => t.Msisdn)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(t => t.SubscriptionDate)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasMany(t => t.Clients);

            builder.HasMany(t => t.Loans);

            builder.HasMany(t => t.Transactions);
        }
    }
}
