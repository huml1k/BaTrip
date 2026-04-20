using BaTrip.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaTrip.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransactionStatus)
                .HasColumnName("Статус")
                .HasConversion<string>()
                .HasColumnType("string")
                .HasMaxLength(25);

            builder.Property(x => x.CreatedDate)
                .IsRequired()
                .HasColumnName("Дата Создания")
                .HasDefaultValueSql("now()");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
