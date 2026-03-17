using BaTrip.Domain.Entities;
using BaTrip.Domain.Enums;
using BaTrip.Domain.Interfaces.Repositories;

namespace BaTrip.Infrastructure.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task Add(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid transactionId, TransactionStatus transactionStatus)
        {
            throw new NotImplementedException();
        }
    }
}
