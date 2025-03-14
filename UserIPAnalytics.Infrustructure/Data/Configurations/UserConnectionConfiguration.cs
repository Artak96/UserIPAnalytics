using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Infrustructure.Data.Configurations
{
    public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.ToTable("Bets");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(uc => uc.IpAddress)
                .IsRequired()
                .HasMaxLength(45);

            builder.Property(uc => uc.UserId)
                .IsRequired();

            builder.Property(b => b.CreatedDate)
              .HasColumnName("CreatedDate");

            builder.Property(b => b.UpdatedDate)
              .HasColumnName("UpdatedDate");

            builder.HasIndex(uc => uc.IpAddress)
         .HasDatabaseName("idx_ip_address");
        }
    }
}
