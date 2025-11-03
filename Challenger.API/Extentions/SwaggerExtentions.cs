using Challenger.Application.Configss;
using Microsoft.OpenApi.Models;

namespace WebApplication2.Extentions;

public static class SwaggerExtentions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, Settings settings)
    {
        services.AddSwaggerGen(swagger =>
        {
           swagger.SwaggerDoc("v1", new OpenApiInfo
           {
               Title = settings.Swagger.Title,
               Version = settings.Swagger.Version,
               Description = settings.Swagger.Description,
               Contact = settings.Swagger.Contact
           });
           
           
           swagger.SwaggerDoc("v2", new OpenApiInfo
           {
               Title = settings.SwaggerV2.Title,
               Version = settings.SwaggerV2.Version,
               Description = settings.SwaggerV2.Description,
               Contact =  settings.SwaggerV2.Contact
           });


           swagger.EnableAnnotations();
        });
        return services;
    }    
}