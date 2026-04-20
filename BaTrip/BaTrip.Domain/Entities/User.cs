using System.ComponentModel.DataAnnotations;

namespace BaTrip.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Favorite> Favorites;
    }
}
