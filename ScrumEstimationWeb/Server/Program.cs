using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScrumEstimationWeb.Server.Entity;
using Microsoft.EntityFrameworkCore.SqlServer;
using ScrumEstimationWeb.Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

ConfigureService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.MapHub<NotifyHub>("/chathub");
app.MapFallbackToFile("index.html");

app.Run();

void ConfigureService()
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
    builder.Services.AddDbContext<DbEntity>(options => options.UseSqlServer(connectionString, opt => opt.CommandTimeout(180)));
    builder.Services.AddSignalR(hubOptions =>
    {
        hubOptions.EnableDetailedErrors = true;
        hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
    });

    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });
}
