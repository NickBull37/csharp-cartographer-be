using csharp_cartographer_be._03.Models.Artifacts;
using Microsoft.EntityFrameworkCore;

namespace csharp_cartographer_be._03.Models
{
    public class CartographerDbContext : DbContext
    {
        public CartographerDbContext(DbContextOptions<CartographerDbContext> options) : base(options)
        {
        }

        // Define DbSets for Cartographer entities
        public DbSet<Artifact> Artifacts { get; set; }
    }
}
