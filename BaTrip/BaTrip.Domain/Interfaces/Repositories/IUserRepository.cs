using BaTrip.Domain.Entities;

namespace BaTrip.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task Add(User user);
        public Task Update(User user);
        public Task Delete(Guid Id);
        public Task<User> GetByEmail(string email);
    }
}
