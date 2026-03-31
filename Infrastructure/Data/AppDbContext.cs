using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Tenancy> Tenancies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entities to table names (match MySQL exactly)
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Property>().ToTable("properties");
            modelBuilder.Entity<Tenancy>().ToTable("tenancies");
            modelBuilder.Entity<Review>().ToTable("reviews");

            // -----------------------------
            // 1️⃣ User-Role relationship
            // -----------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // 2️⃣ Property-Owner relationship
            // -----------------------------
            modelBuilder.Entity<Property>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 3️⃣ Property-Tenancy relationship
            // -----------------------------
            modelBuilder.Entity<Tenancy>()
                .HasOne(t => t.Property)
                .WithMany(p => p.Tenancies)
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 4️⃣ User-Review & Property-Review relationships
            // -----------------------------
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 5️⃣ Default string length and column mapping
            // -----------------------------
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(string)))
                {
                    if (property.GetMaxLength() == null)
                        property.SetMaxLength(255); // default for varchar columns
                }
            }

            // -----------------------------
            // 6️⃣ Configure timestamps (optional)
            // -----------------------------
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            modelBuilder.Entity<Property>()
                .Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}