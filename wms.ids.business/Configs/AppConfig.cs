namespace wms.ids.business.Configs
{
    public class AppConfig
    {
        public static CommonConfig Common;
        public static ConnectionStrings Connection;
        public static JWTConfig JWT;
        public static EmailConfig Email;
    }

    public class CommonConfig
    {
        public string ClientName { get; set; }
        public string Environment { get; set; }
        public int SystemUserID { get; set; }
        public bool DisableAuthen { get; set; }
        public bool DisableSwagger { get; set; }
        public int TFATimeTolerance { get; set; }
        public int LoginTimePerMinute { get; set; }
        public bool UseLoginTimePerMinute { get; set; }
        public string SubcribeChanelNames { get; set; }

        public string DefaultHomePage { get; set; }
        public bool IsCapchaRequired { get; set; }
        public string Issuer { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class JWTConfig
    {
        public string Base64PublicKey { get; set; }

        public string Issuer { get; set; }

        public string Audiences { get; set; }
    }

    public class EmailConfig
    {
        public bool IsUsed { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
