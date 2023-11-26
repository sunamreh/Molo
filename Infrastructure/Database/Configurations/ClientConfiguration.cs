using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;
using System.Net.NetworkInformation;

namespace Molo.Infrastructure.Database.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Msisdn)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(t => t.SubscriberId)
                .IsRequired();

            builder.Property(t => t.DateCreated)
                .IsRequired();

            builder.HasOne(t => t.Subscriber);

            builder.HasMany(t => t.Loans);
        }
    }
}
