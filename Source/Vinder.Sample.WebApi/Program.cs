namespace Vinder.Sample.WebApi;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var environment = builder.Environment;
        var configuration = builder.Configuration;

        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddWebComposition(environment);

        builder.Configuration.AddEnvironmentVariables();

        var app = builder.Build();

        app.UseHttpPipeline();
        app.MapOpenApi();

        await app.RunAsync();
    }
}
