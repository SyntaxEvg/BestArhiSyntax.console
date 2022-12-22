using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace App.DAL
{   /// <summary>
    /// причина почему стал такой DBCONTEXT,мы добавили Identity с простыми настройкAn error occurred while accessing the Microsoft.Extensions.Hosting services. Continuing without the application service provider. Error: Could not load file or assembly 'C:\Users\user\.nuget\packages\microsoft.entityframeworkcore.toolsами пользователей и ролей 
    /// </summary>
    public class AppDBContext : IdentityDbContext<User, Role, string> //DbContext
    {
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        //public virtual DbSet<UsersSession> UsersSession { get; set; }
        //public virtual DbSet<UserRefreshTokens> UserRefreshToken { get; set; }


        public AppDBContext(DbContextOptions<AppDBContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            //Динамичекая регистрация сущностей, чтобы не писать как выше, идея такая. есть базовый класс и по нему находятся и регаются сущности
            //var entitiesAssembly = typeof(EntityName).Assembly;
            //modelBuilder.RegisterAllEntities<entity>(entitiesAssembly);
            //IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic &&
            // typeof(EntityName).IsAssignableFrom(c));
            //foreach (Type type in types)
            //    modelBuilder.Entity(type);

            /// добавить темпоральные таблицы для журнала
            ///тут  конфигурация полей если ef core не справился
        }
    }
}