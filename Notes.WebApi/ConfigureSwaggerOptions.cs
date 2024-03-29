using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebApi
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo()
                    {
                        Version = apiVersion,
                        Title = $"Notes API {apiVersion}",
                        Description = "Простой пример ASP NET Core Web API",
                        TermsOfService = new Uri("https://github.com/def4m3"),
                        Contact = new OpenApiContact()
                        {
                            Name = "def4m3 contact",
                            Email = string.Empty,
                            Url = new Uri("https://t.me/def4m3301")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "def4m3",
                            Url = new Uri("https://github.com/def4m3"),
                        }

                    });
                options.AddSecurityDefinition($"AuthToken {apiVersion}", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "Authorization token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"AuthToken {apiVersion}",
                            }
                        },
                        new string[] { }
                    }
                });


                options.CustomOperationIds(desc =>
                    desc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
            }
        }
    }
}
