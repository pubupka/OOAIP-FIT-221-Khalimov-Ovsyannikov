using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebHttp;

internal sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelWebServices(o =>
        {
            o.Title = "Это заголовок";
        });

        services.AddSingleton(new SwaggerOptions());
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<SwaggerMiddleware>();
        app.UseSwaggerUI();

        app.UseServiceModel(builder =>
        {
            builder.AddService<Endpoint>();
            builder.AddServiceWebEndpoint<Endpoint, IWebApi>(new WebHttpBinding
            {
                MaxReceivedMessageSize = 5242880,
                MaxBufferSize = 65536,
            }, "api", behavior =>
            {
                behavior.HelpEnabled = true;
                behavior.AutomaticFormatSelectionEnabled = true;
            });
        });
    }
}
