using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace wms.infrastructure.Configurations
{
    public static class RegisterServiceContainer
    {
        public static void RegisterServiceDependencyAutofac(this ContainerBuilder builder)
        {
            RegisterInstanceInBusinessProjectToUsingCache(builder);
            RegisterDefaultConnection(builder);
            builder.RegisterConnectionstring(HostBuilderItem.ConnectionStrings);
        }
        private static void RegisterInstanceInBusinessProjectToUsingCache(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.Contains("business") || x.GetName().Name.Contains("infrastructure"));

            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
                }
            }
        }
        private static void RegisterDefaultConnection(ContainerBuilder builder)
        {
            builder.Register(c => new SqlConnection(HostBuilderItem.DefaultConnectionString)).As<IDbConnection>().InstancePerLifetimeScope();
            builder.RegisterType<DapperReadOnlyRepository>().As<IReadOnlyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DapperRepository>().As<IRepository>().InstancePerLifetimeScope();
        }
        public static void RegisterConnectionstring(this ContainerBuilder builder, Dictionary<string, string> dictConnectionstringRegister)
        {
            if (dictConnectionstringRegister == null)
            {
                return;
            }
            foreach (var item in dictConnectionstringRegister)
            {
                var name = item.Key;
                var connectionstring = item.Value;
                builder.Register(c => new SqlConnection(connectionstring)).Keyed<IDbConnection>(name).InstancePerLifetimeScope();
                builder.Register(c =>
                {
                    try
                    {
                        return new DapperReadOnlyRepository(c.ResolveKeyed<IDbConnection>(name));
                    }
                    catch (Exception ex)
                    {
                        //Core.Log.LogIdentify idlog = new();
                        //c.Resolve<Core.Log.Interface.ILogger>().Error(idlog, ex.Message);
                        return null;
                    }
                }).Keyed<IReadOnlyRepository>(name).InstancePerLifetimeScope();

                builder.Register(c =>
                {
                    try
                    {
                        return new DapperRepository(c.ResolveKeyed<IDbConnection>(name));
                    }
                    catch (Exception ex)
                    {
                        //TODO: Add Log here
                        //Core.Log.LogIdentify idlog = new();
                        //c.Resolve<Core.Log.Interface.ILogger>().Error(idlog, ex.Message);
                        return null;
                    }
                }).Keyed<IRepository>(name).InstancePerLifetimeScope();

            }
        }

        public static IServiceCollection RegisterAssemblyTypes<T>(this IServiceCollection services, ServiceLifetime lifetime, List<Func<TypeInfo, bool>> predicates = null)
        {
            var scanAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            scanAssemblies.SelectMany(x => x.GetReferencedAssemblies())
                .Where(t => false == scanAssemblies.Any(a => a.FullName == t.FullName))
                .Distinct()
                .ToList()
                .ForEach(x => scanAssemblies.Add(AppDomain.CurrentDomain.Load(x)));

            var interfaces = scanAssemblies
                .SelectMany(o => o.DefinedTypes
                    .Where(x => x.IsInterface)
                    .Where(x => x != typeof(T))
                    .Where(x => typeof(T).IsAssignableFrom(x))
                );

            foreach (var @interface in interfaces)
            {
                var types = scanAssemblies
                    .SelectMany(o => o.DefinedTypes
                        .Where(x => x.IsClass)
                        .Where(x => @interface.IsAssignableFrom(x))
                    );

                if (predicates?.Count > 0)
                {
                    foreach (var predict in predicates)
                    {
                        types = types.Where(predict);
                    }
                }

                foreach (var type in types)
                {
                    services.TryAdd(new ServiceDescriptor(
                        @interface,
                        type,
                        lifetime)
                    );
                }
            }

            return services;
        }
    }
}
