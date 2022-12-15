using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Policy;
using System.Linq.Expressions;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Расширение помогает автоматически регистрировать DI
    /// </summary>
    public static class NetCoreDIExtension
    {

        /// <summary>
        /// 1 Step: Передать имя сборки(опцинально), иначе будет загружена в данный момент выполняемая,Не рекомендуется держать Service в главном проекте
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeFromAssembly(this Assembly? assems, List<string> AssemblyNameCollection = null)
        {
            if (AssemblyNameCollection == null) return assems.GetTypes().AsEnumerable();
            List<Type> types = new();
            AssemblyNameCollection.Distinct(); //убрать копии
            foreach (var item in AssemblyNameCollection.AsParallel())
            {
                var assemblyName = item.Replace(".dll", "");
                var assemblyPath = Path.Combine(Path.GetDirectoryName(path: Assembly.GetEntryAssembly().Location), assemblyName + ".dll");
                var type = Assembly.Load(AssemblyLoadContext.GetAssemblyName(assemblyPath))?.GetTypes();
                if (type is not null)
                {
                    types.AddRange(type);
                }
            }
            if (types.Count() == 0)
            {
                var t = assems.GetTypes().AsEnumerable();
            }
            return types.AsEnumerable();

        }
        /// <summary>
        /// Internal Method 
        /// </summary>
        /// <param name="typeService">ScopedService,SingletonService,HostedService,TransientService</param>
        private static void RegistrationAuto(this IEnumerable<Type> assemblyType, IEnumerable<Type> filter, IServiceCollection? services)
        {
            var t = filter.Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() }).ToList();
            t.ForEach(typesToRegister =>
            {
                foreach (var item in typesToRegister.serviceTypes)
                {
                    var name = item.Name;
                    var assignedType = typesToRegister.assignedType.Name;
                    if (name.EndsWith("ScopedService") ||
                    assignedType.EndsWith("ScopedRepository") ||
                    assignedType.EndsWith("ScopedService")
                    )
                    {
                        var typeToRegister = item;
                        services.TryAddScoped(typeToRegister, typesToRegister.assignedType);
                    }
                    else if (name.EndsWith("SingletonService") ||
                                                assignedType.EndsWith("SingletonRepository") ||
                                                assignedType.EndsWith("SingletonService"))
                    {
                        var typeToRegister = item;
                        services.TryAddSingleton(typeToRegister, typesToRegister.assignedType);
                    }

                    else if (name.EndsWith("TransientService") ||
                             assignedType.EndsWith("TransientRepository") ||
                             assignedType.EndsWith("TransientService"))
                    {
                        var typeToRegister = item;
                        services.TryAddTransient(typeToRegister, typesToRegister.assignedType);
                    }
                }
            });
        }
        /// <summary>
        /// Авторегистрация Сервисов,Опциональный параметр,если он задан, будет регистрироваться только выбранный сервис,возможные варианты ScopedService,SingletonService,HostedService,TransientService
        /// </summary>
        public static void RegistrationAutoOrManual(this IEnumerable<Type> assemblyType, IServiceCollection? services, string? typeService = null)
        {
            IEnumerable<Type> filter;
            if (typeService is not null)
            {
                IQueryable<Type> d = assemblyType.AsQueryable().Where((a => a.Name.EndsWith(typeService) && !a.IsAbstract && !a.IsInterface));
            }
            filter = assemblyType.Where(a => (a.Name.EndsWith("ScopedService") ||
                                         a.Name.EndsWith("SingletonService") ||
                                         // a.Name.EndsWith("HostedService") ||
                                         a.Name.EndsWith("TransientService") ||
                                         a.Name.EndsWith("ScopedRepository") ||
                                         a.Name.EndsWith("SingletonRepository") ||
                                         a.Name.EndsWith("TransientRepository")
                                         ) && !a.IsAbstract && !a.IsInterface);

            assemblyType.RegistrationAuto(filter, services);
        }
    }
}
