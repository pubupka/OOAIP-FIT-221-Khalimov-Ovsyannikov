using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebHost.CreateDefaultBuilder(args)
    .UseKestrel(options =>
    {
        options.AllowSynchronousIO = true;
        options.ListenAnyIP(156789);
    })
    .UseStartup<Startup>();

var app = builder.Build();
app.Run();
