using BaTrip.Domain.Entities;
using BaTrip.Domain.Interfaces.Repositories;

namespace BaTrip.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task Add(User user)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
