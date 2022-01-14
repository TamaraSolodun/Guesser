using Microsoft.EntityFrameworkCore;
using DataLayer_Guesser.Models;

namespace DataLayer_Guesser
{
    public class GuesserDBContext : DbContext
    {
        public GuesserDBContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity => entity
                .HasMany(u => u.Games)
                .WithOne(t => t.Player)
                .HasForeignKey(t => t.PlayerId)
                .OnDelete(DeleteBehavior.Cascade));

            base.OnModelCreating(modelBuilder);
        }
    }
}
