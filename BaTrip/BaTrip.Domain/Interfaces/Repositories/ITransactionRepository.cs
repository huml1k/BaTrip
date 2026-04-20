using BaTrip.Domain.Entities;
using BaTrip.Domain.Enums;


namespace BaTrip.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        public Task Add(Transaction transaction);
        public Task Update(Transaction transaction);
        public Task Delete(Guid transactionId);
        public Task<IEnumerable<Transaction>> GetAll();
        public Task<IEnumerable<Transaction>> GetByUserId(Guid userId);
    }
}
