using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.DAL.Postgres
{
    public static class Registrator
    {
        public static void UseNpgsql(this IServiceCollection services, string connection_string)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Для работы с датой и временем в Postgres //https://www.npgsql.org/doc/types/datetime.html
            services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(connection_string, o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));
        }
    }
}