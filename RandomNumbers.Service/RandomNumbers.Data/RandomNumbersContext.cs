using RandomNumbers.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace RandomNumbers.Data
{
    public class RandomNumbersContext : DbContext
    {
        public RandomNumbersContext(DbContextOptions<RandomNumbersContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<Match>().ToTable(nameof(Match));
            modelBuilder.Entity<UserMatch>().ToTable(nameof(UserMatch));

            modelBuilder.Entity<UserMatch>()
                .HasOne(um => um.User)
                .WithMany()
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMatch>()
                .HasOne(um => um.Match)
                .WithMany()
                .HasForeignKey(um => um.MatchId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.WinnerUser)
                .WithMany()
                .HasForeignKey(m => m.WinnerUserId);
        }
    }
}
