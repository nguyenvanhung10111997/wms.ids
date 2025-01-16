using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace wms.infrastructure.Configurations
{
    public partial class HostBuilderItem
    {
        private static IServiceCollection _serviceCollectonItem;
        private static IApplicationBuilder _applicationBuilderItem;
        private static ContainerBuilder _containerBuilderItem;
        private static IConfiguration _configurationItem;

        public static IConfiguration ConfigurationItem { get { return _configurationItem; } set { _configurationItem = value; } }

        public static string DefaultConnectionString { get; set; }

        public static Dictionary<string, string> ConnectionStrings { get; set; }

        public static string AcceptedIssuer { get; set; }

        public static bool DisableAuthorise { get; set; } = false;

        public static string Enviroment { get; set; } = "production";

        public static ContainerBuilder ContainerBuilderItem
        {
            get { return _containerBuilderItem; }
            set
            {
                if (_containerBuilderItem == null)
                {
                    _containerBuilderItem = value;
                    var builder = _containerBuilderItem;
                    SetupContainerBuilder(builder);
                    return;
                }
                throw (new Exception("Fatal error only one container builder be created on running."));
            }
        }

        public static IServiceCollection ServiceCollectionItem
        {
            get { return _serviceCollectonItem; }
            set
            {
                if (_serviceCollectonItem == null)
                {
                    _serviceCollectonItem = value;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    SetupServiceCollection(_serviceCollectonItem);
                    return;
                }
                Console.Write("Supper fatal error, one init for one host service collection.");
            }
        }

        public static IApplicationBuilder ApplicationBuilderItem
        {
            get { return _applicationBuilderItem; }
            set
            {
                if (_applicationBuilderItem == null)
                {
                    _applicationBuilderItem = value;
                    InitApplicationBuilderItem();
                    return;
                }
                Console.Write("Supper fatal error, one init for one host builder.");
            }
        }
    }
}
