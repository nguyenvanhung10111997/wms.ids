using Microsoft.Extensions.Configuration;
using wms.ids.business.Configs;

namespace wms.ids.business.Configs
{
    public static class AppSettingRegister
    {
        public static void Binding(IConfiguration configuration)
        {
            AppConfig.Common = new CommonConfig();
            configuration.Bind("CommonConfig", AppConfig.Common);

            AppConfig.Connection = new ConnectionStrings();
            configuration.Bind("ConnectionStrings", AppConfig.Connection);

            AppConfig.JWT = new JWTConfig();
            configuration.Bind("JWT", AppConfig.JWT);

            AppConfig.Email = new EmailConfig();
            configuration.Bind("EmailConfig", AppConfig.Email);
        }
    }
}
