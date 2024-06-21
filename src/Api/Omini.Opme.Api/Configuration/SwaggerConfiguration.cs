using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Omini.Opme.Api.Configuration;

internal static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        var scopes = configuration.GetSection("Auth0:Scopes").Get<List<string>>()!.ToDictionary(p => p, v => string.Empty);

        services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<SwaggerDefaultValues>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insert your bearer token as: Bearer {your token}",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{configuration["Auth0:Authority"]}/authorize?audience={configuration["Auth0:Audience"]}"),
                            TokenUrl = new Uri($"{configuration["Auth0:Authority"]}/oauth/token"),
                            Scopes = scopes
                        },
                    },
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                            },
                            scopes.Keys.ToArray()
                        }
                    });
            });

        return services;
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
                return;

            // foreach (var parameter in operation.Parameters)
            // {
            //     var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
            //     if (parameter.Description == null)
            //     {
            //         parameter.Description = description.ModelMetadata?.Description;
            //     }

            //     if (parameter.Schema.Default == null && description.DefaultValue != null)
            //     {
            //         parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
            //     }

            //     parameter.Required |= description.IsRequired;
            // }
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Opme API",
                Description = "API to consume Opme application",
                Contact = new OpenApiContact() { Name = "Zenko team", Email = "hello@zenko.tec.br" },
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
