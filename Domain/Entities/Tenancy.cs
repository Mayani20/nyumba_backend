using System;

namespace Domain.Entities
{
    public class Tenancy
    {
        public int Id { get; set; }
        public int UserId { get; set; }              // Tenant
        public User User { get; set; } = null!;
        public int PropertyId { get; set; }          // Rented property
        public Property Property { get; set; } = null!;
        public DateTime StartDate { get; set; }      // Lease start
        public DateTime EndDate { get; set; }        // Lease end
        public decimal RentAmount { get; set; }
    }
}