using System;

namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }              // Reviewer
        public User User { get; set; } = null!;
        public int PropertyId { get; set; }          // Reviewed property
        public Property Property { get; set; } = null!;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }              // e.g., 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}