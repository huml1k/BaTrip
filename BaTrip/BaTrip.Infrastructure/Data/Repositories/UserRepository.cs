using BaTrip.Domain.Entities;
using BaTrip.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaTrip.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var user = await _appDbContext.Users.FindAsync(Id);
            if (user != null)
            {
                _appDbContext.Remove(user);
                await _appDbContext.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _appDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
            if (user != null) 
            {
                return user;
            }
            throw new NotImplementedException();
        }

        public async Task Update(User user)
        {
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
