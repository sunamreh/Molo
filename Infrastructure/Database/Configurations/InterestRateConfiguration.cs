using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Molo.Domain.Entities;

namespace Molo.Infrastructure.Database.Configurations
{
    public class InterestRateConfiguration : IEntityTypeConfiguration<InterestRate>
    {
        public void Configure(EntityTypeBuilder<InterestRate> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Percentage)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(
                new InterestRate { Id = 1, Percentage = 5, Description = "5 Percent" },
                new InterestRate { Id = 2, Percentage = 10, Description = "10 Percent" },
                new InterestRate { Id = 3, Percentage = 15, Description = "15 Percent" },
                new InterestRate { Id = 4, Percentage = 20, Description = "20 Percent" },
                new InterestRate { Id = 5, Percentage = 25, Description = "25 Percent" },
                new InterestRate { Id = 6, Percentage = 30, Description = "30 Percent" }
            );
        }
    }
}
