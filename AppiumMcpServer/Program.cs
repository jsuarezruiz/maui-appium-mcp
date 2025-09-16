using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithResourcesFromAssembly()
                .WithToolsFromAssembly();

            builder.Logging.ClearProviders();

            var app = builder.Build();

            await app.RunAsync();

            return 0;
        }
        catch (Exception)
        {
            return 1;
        }
    }
}