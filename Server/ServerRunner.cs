using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VSystem.Internal.Keys;
using VSystem.Tools;

namespace VSystem.Server;

public class ServerRunner
{
    private readonly RunServerKey _runKey;

    public ServerRunner()
    {
        _runKey = KeyGetTool.GetKey<RunServerKey>();
    }

    public async Task RunAsync(string[] args, CancellationToken cancellationToken)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls(CreateServerUrl());
        builder.Services.AddControllers();

        WebApplication app = builder.Build();

        app.MapControllers();

        /*_loggingService.LogMessage(
            message: string.Format(AppConstants.Server.SuccessStartingServerMessage, _settings.BaseUrl),
            sender: this);*/

        await ((IHost)app).RunAsync(cancellationToken);
    }

    private string CreateServerUrl()
    {
        return $"http://{_runKey.Host}:{_runKey.Port}";
    }
}
