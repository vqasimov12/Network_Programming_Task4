using Microsoft.EntityFrameworkCore;

namespace ServerSide;
public class AppDataContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public AppDataContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;");
    }
}
