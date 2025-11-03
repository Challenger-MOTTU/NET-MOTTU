namespace Challenger.Application.Configss;

public class Settings
{
    public ConnectionSettings ConnectionStrings { get; set; }
    
    public SwaggerSettings Swagger { get; set; }
    public SwaggerSettings SwaggerV2 { get; set; }
}