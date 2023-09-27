using API_ESP_GW.Models;
using Microsoft.EntityFrameworkCore;

namespace API_ESP_GW
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions options):base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BarCode>().Property(prop => prop.Pos)
                .HasDefaultValue(0);
        }
        public DbSet<BarCode> BarCodes { get; set; }
    }
}
