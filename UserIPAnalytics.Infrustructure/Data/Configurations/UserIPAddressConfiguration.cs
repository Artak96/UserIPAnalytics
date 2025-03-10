using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Infrustructure.Data.Configurations
{
    public class UserIPAddressConfiguration : IEntityTypeConfiguration<UserIPAddress>
    {
        public void Configure(EntityTypeBuilder<UserIPAddress> builder)
        {
            builder.ToTable("Bets");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.IPAddress)
                .HasColumnName("IPAddress");

            builder.Property(b => b.UserId)
                .HasColumnName("UserId");

            builder.Property(b => b.CreatedDate)
              .HasColumnName("CreatedDate");

            builder.Property(b => b.UpdatedDate)
              .HasColumnName("UpdatedDate");
        }
    }
}
