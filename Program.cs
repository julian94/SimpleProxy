using Microsoft.Extensions.FileProviders;
using Serilog;
using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Information("Started Proxy Server");
builder.Host.UseSerilog();

builder.Services.AddCors();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

/*
app.MapGet("/honey/{n}.html", (int n) => 
    $"""
    <!DOCTYPE html>
    <html lang="en">
      <head>
        <meta charset="UTF-8" />
        <title>X</title>
      </head>
      <body>
        <a href="./{n+1}.html">Next</a>
      </body>
    </html>
    """);*/

app.UseStaticFiles();
app.MapReverseProxy();

app.Run();
