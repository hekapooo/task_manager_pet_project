using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManager.Models;

namespace TaskManager.Services;

public class UnitOfWork : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!Directory.Exists("Database"))
        {
            Directory.CreateDirectory("Database");
        }

        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("MyConnection"));
    }
}
