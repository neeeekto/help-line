using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HelpLine.Apps.Admin.API.Configuration.Extensions
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(options =>
            {
                options.UseAllOfForInheritance();
                options.UseOneOfForPolymorphism();
                options.SelectDiscriminatorNameUsing((baseType) => "$type");
                options.SelectDiscriminatorValueUsing((subType) => subType.Name);

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HelpLine Admin API",
                    Version = "v1",
                    Description = "HelpLine Admin API",
                });

                var oauthEndpoint = configuration["Auth:Authority"];

                options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{oauthEndpoint}/connect/authorize"),
                            TokenUrl = new Uri($"{oauthEndpoint}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { configuration["Auth:ApiName"], "API" }
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "OAuth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

            });
            return services;
        }
        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelpLine Admin API");
                c.OAuthClientId(configuration["Swagger:Auth:ClientId"]);
                c.OAuthClientSecret(configuration["Swagger:Auth:Secret"]);
                c.OAuthScopes(configuration["Auth:ApiName"]);
                c.OAuthAppName("Web API");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
            });
            return app;
        }
    }
}
