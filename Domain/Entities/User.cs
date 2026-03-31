// Domain/Entities/User.cs
using System;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        // Foreign keys
        public int RoleId { get; set; }
        
        // Navigation properties
        public Role? Role { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Tenancy>? Tenancies { get; set; }
        
        // ✅ Add these missing properties
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}