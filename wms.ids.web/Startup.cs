// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Autofac;
using wms.ids.web.Configuration;
using wms.ids.web.Configuration.IDS;
using IdentityServerHost.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using wms.infrastructure.Configurations;
using wms.ids.business.Configs;
using wms.ids.business.Services.Implement;

namespace wms.ids.web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            HostBuilderItem.ConfigurationItem = configuration;
            AppSettingRegister.Binding(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            HostBuilderItem.ServiceCollectionItem = services;

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.EmitScopesAsSpaceDelimitedStringInJwt = true;
                options.IssuerUri = AppConfig.Common.Issuer;
            })
                .AddResourceStore<IDSResourceStore>()
                .AddClientStore<IDSClientStore>()
                .AddResourceOwnerValidator<IDSPasswordValidator>()
                .AddProfileService<IDSProfileService>()
                .AddSigningCredential(CertificateLoader.LoadCertificate());

            services.AddSameSiteCookiePolicy();

            services.AddAuthentication();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddScoped<CaptchaVerificationService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            HostBuilderItem.ContainerBuilderItem = builder;
            builder.RegisterModule<AutoFacModule>();
            builder.RegisterDBConnection();
        }

        public void Configure(IApplicationBuilder app)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain,
                  SslPolicyErrors sslPolicyErrors)
            { return true; };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                });
            }
            else
            {
                app.UseStaticFiles();
            }

            ServiceActivator.Configure(app.ApplicationServices);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCertificateForwarding();
            app.UseCookiePolicy();
            app.UseDeveloperExceptionPage();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.ToString() == "/")
                {
                    context.Response.Redirect("/PageNotFound.html");
                }
                else
                {
                    await next();
                }
            });

            app.UseSession();
            app.UseRouting();
            app.UseIdentityServer(new IdentityServerMiddlewareOptions { });
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            HostBuilderItem.ApplicationBuilderItem = app;
        }
    }
}