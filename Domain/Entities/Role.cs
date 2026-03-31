using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }                   // Primary key
        public string Name { get; set; } = string.Empty; // Role name, e.g., Admin, Tenant, Landlord

        // Navigation property - one role can have many users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}