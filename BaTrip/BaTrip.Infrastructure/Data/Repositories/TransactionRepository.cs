using BaTrip.Domain.Entities;
using BaTrip.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaTrip.Infrastructure.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _appDbContext;

        public TransactionRepository(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(Transaction transaction)
        {
            if (transaction != null) 
            {
                await _appDbContext.Transactions.AddAsync(transaction);
                await _appDbContext.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }

        public async Task Delete(Guid transactionId)
        {
            var transaction = await _appDbContext.Transactions.FindAsync(transactionId);
            if (transaction != null) 
            {
                _appDbContext.Remove(transaction);
                await _appDbContext.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            var result = _appDbContext.Transactions
                .AsNoTracking()
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
            if (result != null) 
            {
                return await result;
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetByUserId(Guid userId)
        {
            var result = await _appDbContext.Transactions
                .AsNoTracking()
                .OrderBy(x => x.CreatedDate)
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if (result != null) 
            {
                return result;
            }
            throw new NotImplementedException();
        }

        public async Task Update(Transaction transaction)
        {
            _appDbContext.Transactions.Update(transaction);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
