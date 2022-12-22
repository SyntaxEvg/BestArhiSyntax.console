using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.DAL.SqLite
{
    public static class Registrator
    {
        public static void UseSqlite(this IServiceCollection services, string connection_string)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlite(connection_string, o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));
        }
    }
}