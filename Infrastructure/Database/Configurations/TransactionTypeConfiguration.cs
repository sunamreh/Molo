using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;

namespace Molo.Infrastructure.Database.Configurations
{
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
    {
        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(
                new TransactionType { Id = 1, Description = "Credit" },
                new TransactionType { Id = 2, Description = "Debit" }
            );
        }
    }
}
