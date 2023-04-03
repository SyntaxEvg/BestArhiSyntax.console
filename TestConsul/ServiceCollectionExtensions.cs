using Consul;

namespace TestConsul
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Конфугурация Consul и IConsulClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="url"></param>
        /// <param name="datacenter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection UseConul(this IServiceCollection services, string url, string datacenter = "DC")
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);

            }));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new
            ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);
                consulConfig.Datacenter = datacenter;
                //consulConfig.Token = "X-Consul-Token";
            }));
            services.AddHostedService<RegisterConsulHostedService>();
            return services;
        }

    }
}
//docker run -d -p 8500:8500 -p 8600:8600/udp --name=my-consul consul agent -server -ui -node=server-1 -bootstrap-expect=1 -client=0.0.0.0

