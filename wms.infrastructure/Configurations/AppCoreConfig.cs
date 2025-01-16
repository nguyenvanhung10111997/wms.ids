namespace wms.infrastructure
{
    public class AppCoreConfig
    {
        public static CommonConfig Common;
        public static ConnectionStrings Connection;
        public static URLConnectionConfig URLConnection;
        public static JWTConfig JWT;
        public static ProviderConfig Providers;
    }

    public class CommonConfig
    {
        public string ClientName { get; set; }
        public string Environment { get; set; }
        public int SystemUserID { get; set; }
        public bool DisableAuthen { get; set; }
        public bool DisableSwagger { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class URLConnectionConfig
    {
        public string IDSUrl { get; set; }
    }

    public class JWTConfig
    {
        public string Base64PublicKey { get; set; }

        public string Issuer { get; set; }

        public string Audiences { get; set; }
    }

    public class ProviderConfig
    {
        public RabbitMQConfig RabbitMQ { get; set; }
    }

    public class RabbitMQConfig
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
