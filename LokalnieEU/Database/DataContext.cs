using LokalnieEU.Models.User;
using Microsoft.EntityFrameworkCore;

namespace LokalnieEU.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEmails>().HasNoKey();
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<RoleEmails> RoleEmails => Set<RoleEmails>();
    }
}
