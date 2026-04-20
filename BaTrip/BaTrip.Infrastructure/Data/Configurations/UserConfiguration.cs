using BaTrip.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaTrip.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("Почта")
                .HasColumnType("string");

            builder.Property(x => x.Phone)
                .HasColumnName("Телефон")
                .HasColumnType("int")
                .HasMaxLength(12);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasColumnName("Имя")
                .HasColumnType("string")
                .HasMaxLength(32);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasColumnName("Фамилия")
                .HasColumnType("string")
                .HasMaxLength(20);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasColumnName("Пароль")
                .HasColumnType("string")
                .HasMaxLength(16);

            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Favorites)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
 