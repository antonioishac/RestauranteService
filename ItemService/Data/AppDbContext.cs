using ItemService.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Restaurante> Restaurants { get; set; }
        public DbSet<Item> Items { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Restaurante>()
                .HasMany(c => c.Itens)
                .WithOne(a => a.Restaurante!)
                .HasForeignKey(a => a.Restaurante);

            modelBuilder
                .Entity<Item>()
                .HasOne(a => a.Restaurante)
                .WithMany(c => c.Itens)
                .HasForeignKey(a => a.IdRestaurante);
        }

    }
}
