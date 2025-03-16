using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Infrustructure.Data.Configurations
{
    public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.ToTable("UserConnections");

            // Primary Key
            builder.HasKey(uc => uc.Id);

            // Properties
            builder.Property(uc => uc.IpAddress)
                .IsRequired() // Ensure IpAddress is required
                .HasMaxLength(45); // IPv6 maximum length

            // Foreign Key to User
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserConnections)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when a user is deleted
            builder.Property(b => b.CreatedDate)
              .HasColumnName("CreatedDate");

            builder.Property(b => b.UpdatedDate)
              .HasColumnName("UpdatedDate");

            builder.HasIndex(uc => uc.IpAddress)
         .HasDatabaseName("idx_ip_address");
        }
    }
}
