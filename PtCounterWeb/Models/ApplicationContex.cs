using Microsoft.EntityFrameworkCore;

namespace PtCounterWeb.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Report> Reports { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
