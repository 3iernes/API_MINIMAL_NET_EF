using API_ESP_GW.Models;
using Microsoft.EntityFrameworkCore;

namespace API_ESP_GW
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions options):base(options) { }
        public DbSet<BarCode> BarCodes { get; set; }
    }
}
