using creditPreCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace creditPreCheck.Data
{
    public class CreditPreCheckContext : DbContext
    {
        public CreditPreCheckContext(DbContextOptions<CreditPreCheckContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Eligibility> Eligibility { get; set; }
    }
}
