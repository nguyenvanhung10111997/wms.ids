using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using wms.infrastructure.Attributes;
using wms.infrastructure.Exceptions;
using wms.infrastructure.Middlewares;
using wms.infrastructure.Swagger;
using wms.infrastructure.Validator;

namespace wms.infrastructure.Configurations
{
    public static partial class HostBuilderItem
    {
        private static void InitApplicationBuilderItem()
        {
            Engine.ContainerManager = new ContainerManager(_applicationBuilderItem.ApplicationServices.GetAutofacRoot());
            _applicationBuilderItem.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = exception.Message
                });
                Console.Write($"Erorr message:{exception.Message}. Error message detail: {exception.InnerException.Message}");
            }));

            _applicationBuilderItem.ConfigSwagger(_configurationItem);
            _applicationBuilderItem.ConfigApp();
            _applicationBuilderItem.UseHttpsRedirection();
            _applicationBuilderItem.UseRouting();
            _applicationBuilderItem.UseAuthorization();

            //setup usestaticfile
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _applicationBuilderItem.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")) });
            }
            else { _applicationBuilderItem.UseStaticFiles(); }

            _applicationBuilderItem.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/reload/appsetting", async context =>
                {
                    try
                    {
                        SetupServiceCollection(_serviceCollectonItem);
                        SetupContainerBuilder(_containerBuilderItem);
                        InitApplicationBuilderItem();
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync("Finished");
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync($"Failed to reload appsetting {ex.Message}");
                    }

                });
                // The readiness check uses all registered checks with the 'ready' tag.
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResultStatusCodes = {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    // Exclude all checks and return a 200-Ok.
                    Predicate = (_) => false,
                    ResultStatusCodes = {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
                endpoints.MapControllers();
            });

        }

        private static void SetupContainerBuilder(ContainerBuilder builder)
        {
            RegisterHttpClient(builder);
            builder.RegisterServiceDependencyAutofac();

        }

        private static void RegisterHttpClient(ContainerBuilder builder)
        {
            builder.Register(c => new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })).As<HttpClient>().InstancePerLifetimeScope();
        }

        private static void SetupServiceCollection(IServiceCollection services)
        {
            AppCoreSettingRegister.Binding(_configurationItem);

            services.AddControllers();
            services.AddHealthChecks();
            services.AddSingleton(_configurationItem);
            services.ConfigServices(_configurationItem);
            services.SwaggerConfig();

            services.AddMvcCore(o =>
            {
                o.Filters.Add<ValidationResultAttribute>();
            }).AddAuthorization();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            }).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.InitFluentValidation();

            Authentication(_serviceCollectonItem);
        }

        private static void Authentication(IServiceCollection services)
        {
            //if (!HostBuilderItem.DisableAuthorise)
            //{
            //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //    {
            //        var cert = new X509Certificate2(Convert.FromBase64String(AppCoreConfig.JWT.Base64PublicKey));
            //        SecurityKey securityKey = new X509SecurityKey(cert);
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // Clock skew compensates for server time drift.
            //            // We recommend 5 minutes or less:
            //            ClockSkew = TimeSpan.FromMinutes(5),
            //            // Specify the key used to sign the token:
            //            IssuerSigningKey = securityKey,
            //            RequireSignedTokens = true,
            //            // Ensure the token hasn't expired:
            //            RequireExpirationTime = true,
            //            ValidateLifetime = true,
            //            // Ensure the token audience matches our audience value (default true):
            //            ValidateAudience = false,
            //            //ValidAudiences = ApiConfig.JWT.Audiences.Split(','),                    
            //            // Ensure the token was issued by a trusted authorization server (default true):
            //            ValidateIssuer = true,
            //            ValidIssuers = new List<string> { AppCoreConfig.JWT.Issuer, HostBuilderItem.AcceptedIssuer }
            //        };
            //    });
            //}
        }

        private static void ConfigServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(new GlobalExceptionFilter());
            });

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        private static void AllowUseMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] TMiddleware>(this IApplicationBuilder app, params object[] args)
        {
            app.UseMiddleware<TMiddleware>();
        }

        private static void ConfigApp(this IApplicationBuilder app)
        {
            Engine.ContainerManager = new ContainerManager(app.ApplicationServices.GetAutofacRoot());
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseAuthentication();
        }
    }
}