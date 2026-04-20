using BaTrip.Domain.Enums;

namespace BaTrip.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
