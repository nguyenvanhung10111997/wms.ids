using Autofac;
using wms.ids.business.Configs;
using wms.infrastructure.Configurations;

namespace wms.ids.web.Configuration
{
    public static class ConnectionDBConfig
    {
        public static void RegisterDBConnection(this ContainerBuilder builder)
        {
            HostBuilderItem.DefaultConnectionString = AppConfig.Connection.DefaultConnection;

            // Other DB connections
            var otherConnections = new Dictionary<string, string>();
            HostBuilderItem.ConnectionStrings = otherConnections;
        }
    }
}
