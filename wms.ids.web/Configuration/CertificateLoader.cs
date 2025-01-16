using System.Security.Cryptography.X509Certificates;


namespace wms.ids.web.Configuration
{
    public static class CertificateLoader
    {
        public static X509Certificate2 LoadCertificate()
        {
            try
            {
                var separator = Path.DirectorySeparatorChar;
                //var filePath = string.Format($"{Directory.GetCurrentDirectory()}{separator}wwwroot{separator}CERT{separator}CERT_wms.pfx");
                var filePath = string.Format($"{Directory.GetCurrentDirectory()}{separator}wwwroot{separator}CERT{separator}identityserver.pfx");
                //return new X509Certificate2(filePath, "14121986");
                return new X509Certificate2(filePath, "0123321");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}