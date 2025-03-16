using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Infrustructure.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Name)
                .IsRequired() // Ensure the Name is required
                .HasMaxLength(100); // Limit Name length


            builder.Property(b => b.CreatedDate)
            .HasColumnName("CreatedDate");

            builder.Property(b => b.UpdatedDate)
              .HasColumnName("UpdatedDate");
        }
    }
}
