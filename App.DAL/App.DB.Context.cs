using App.DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace App.DAL
{
    public class AppDBContext :DbContext
    {
        DbSet<Employees> Employees { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<OrderItem> OrderItem { get; set; }
        DbSet<Orders> Orders { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> opt ):base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            ///тут  конфигурация полей если ef core не справился
        }
    }
}