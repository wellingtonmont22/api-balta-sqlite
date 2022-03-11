using Microsoft.EntityFrameworkCore;
using T.Models;

namespace T.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
        }

    }
}
