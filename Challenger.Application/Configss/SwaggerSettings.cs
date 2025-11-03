using Microsoft.OpenApi.Models;

namespace Challenger.Application.Configss;

public class SwaggerSettings
{
    public string Title { get; init; }

    public string Description { get; init; }
    
    public string Version { get; init; }

    public OpenApiContact Contact { get; set; }
}