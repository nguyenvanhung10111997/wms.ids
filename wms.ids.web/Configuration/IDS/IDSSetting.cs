using wms.ids.business.Configs;
using wms.ids.dto;

namespace wms.ids.web.Configuration
{
    public static class IDSSetting
    {
        public static IEnumerable<string> SubcribeChanelNames => AppConfig.Common.SubcribeChanelNames.Split(';');
        public static List<LockedUserModel> LockedUsers;
        public static int TFATimeTolerance => AppConfig.Common.TFATimeTolerance;
        public static string EmailAddress => AppConfig.Email.Address;
        public static string EmailPassword => AppConfig.Email.Password;
        public static string EmailHost => AppConfig.Email.Host;
        public static string EmailPort => AppConfig.Email.Port.ToString();
        public static List<LoginCountUserModel> UserLoginFails;
        public static int LoginTimePerMinute => AppConfig.Common.LoginTimePerMinute;
        public static bool UseLoginTimePerMinute => AppConfig.Common.UseLoginTimePerMinute;
    }
}