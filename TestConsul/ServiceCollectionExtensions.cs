using Consul;

namespace TestConsul
{
    public static class ServiceCollectionExtensions
    { 
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, string configKey)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(configKey);

            }));

            return services;
        }
    }
}
//docker run -d -p 8500:8500 -p 8600:8600/udp --name=my-consul consul agent -server -ui -node=server-1 -bootstrap-expect=1 -client=0.0.0.0

