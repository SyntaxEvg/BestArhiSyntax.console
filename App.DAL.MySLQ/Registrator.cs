using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.DAL.MySLQ
{
    public static class Registrator
    {
        public static void UseSqlServer(this IServiceCollection services,string connection_string)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connection_string, o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));
        }
    }
}