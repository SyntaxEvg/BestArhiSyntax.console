using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL
{   /// <summary>
    /// причина почему стал такой DBCONTEXT,мы добавили Identity с простыми настройкAn error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: Could not load file or assembly 'C:\Users\user\.nuget\packages\microsoft.entityframeworkcore.toolsами пользователей и ролей 
    /// </summary>
    public class AppDBContext : IdentityDbContext<User, Role, string> //DbContext
    {
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }

        //public AppDBContext()
        //{

        //}

        public AppDBContext(DbContextOptions<AppDBContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            ///тут  конфигурация полей если ef core не справился
        }
    }
}