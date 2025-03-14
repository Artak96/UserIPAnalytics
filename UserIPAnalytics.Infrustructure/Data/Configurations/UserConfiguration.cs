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

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(u => u.UserConnections)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.CreatedDate)
            .HasColumnName("CreatedDate");

            builder.Property(b => b.UpdatedDate)
              .HasColumnName("UpdatedDate");
        }
    }
}
