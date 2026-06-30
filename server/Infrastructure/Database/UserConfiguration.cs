using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(Roles.USER);

            builder.Property(u => u.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(u => u.ConfirmedEmailAt)
                .HasColumnType("datetime2")
                .IsRequired(false);
        }
    }
}
