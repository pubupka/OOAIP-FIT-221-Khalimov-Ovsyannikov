using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

internal class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        var builder = WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            {
                options.AllowSynchronousIO = true;
                options.ListenAnyIP(156789);
            })
            .UseStartup<Startup>();

        var app = builder.Build();
        app.Run();
    }
}
