// Domain/Entities/Property.cs
using System;

namespace Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double Area { get; set; }
        public string Status { get; set; } = "available"; // available, rented, maintenance
        
        // ✅ Add this missing property
        public int OwnerId { get; set; }
        
        // ✅ Add this missing property
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public User? Owner { get; set; }
        public ICollection<Tenancy>? Tenancies { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}