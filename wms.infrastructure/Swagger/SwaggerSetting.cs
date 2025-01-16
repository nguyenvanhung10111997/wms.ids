using wms.infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace wms.infrastructure.Swagger
{
    public static class SwaggerSetting
    {
        public static void ConfigSwagger(this IApplicationBuilder app, IConfiguration Configuration)
        {
            if (!AppCoreConfig.Common.DisableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
                    //options.OAuthClientId(AppCoreConfig.OauthSwagger.ClientName);
                    //options.OAuthAppName(AppCoreConfig.OauthSwagger.ClientName);
                    //options.OAuthClientSecret(AppCoreConfig.OauthSwagger.ClientSecret);
                    options.DefaultModelsExpandDepth(-1);
                });
            }
        }
        public static void SwaggerConfig(this IServiceCollection services)
        {
            if (!AppCoreConfig.Common.DisableSwagger)
            {
                var domainName = AppDomain.CurrentDomain.FriendlyName;
                Dictionary<string, string> swaggerScopes = new Dictionary<string, string>();
                //foreach (var scope in AppCoreConfig.OauthSwagger.Scopes.Split(','))
                //{
                //    if (!string.IsNullOrWhiteSpace(scope))
                //        swaggerScopes.Add(scope, $"access to {scope}");
                //}
                services.AddSwaggerGen((Action<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>)(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = domainName, Description = $"Swagger {domainName}", Version = "v1" });
                    var xmlFile = $"{domainName}.xml";
                    var xmlPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    //{
                    //    Type = SecuritySchemeType.OAuth2,
                    //    Flows = new OpenApiOAuthFlows
                    //    {
                    //        Implicit = new OpenApiOAuthFlow
                    //        {
                    //            AuthorizationUrl = new Uri($"{AppCoreConfig.URLConnection.IDSUrl}/connect/authorize"),
                    //            Scopes = swaggerScopes,
                    //            TokenUrl = new Uri($"{AppCoreConfig.URLConnection.IDSUrl}/connect/token"),
                    //        }
                    //    },
                    //    In = ParameterLocation.Header,
                    //    Name = "Authorization",
                    //    Scheme = "Bearer",
                    //});

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Scheme = "Bearer",
                    });


                    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            //new OpenApiSecurityScheme
                            //{
                            //    Reference = new OpenApiReference
                            //    {
                            //        Type = ReferenceType.SecurityScheme,
                            //        Id = "Bearer"
                            //    },
                            //    Scheme = "Bearer",
                            //    Type = SecuritySchemeType.Http,
                            //    Name = "Bearer",
                            //    In = ParameterLocation.Header
                            //},
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "Bearer",
                                Type = SecuritySchemeType.ApiKey,
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new string[] { }
                        }
                });
                    options.CustomOperationIds(e => $"{e.RelativePath}");

                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                }));
            }
        }

    }
}
