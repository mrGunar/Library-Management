using AutoMapper;
using LibraryManagement.Api;
using LibraryManagement.Api.Services;
using LibraryManagement.Application;
using LibraryManagement.Infrastructure;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options => 
{ 
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 1024;
    options.MaxSendMessageSize = 1024;
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File($"logs/log_{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}.txt")
    .CreateLogger();


var container = new Container();
container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

builder.Services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore();
});


var options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
      .Options;

container.AddApplicationLayer();
container.AddInfrastructureLayer(options);

// Seed the db
//container.SeedDatabaseAsync();

container.Register<AuthorGrpcService>(Lifestyle.Scoped);
container.Register<BookGrpcService>(Lifestyle.Scoped);
container.Register<BorrowingGrpcService>(Lifestyle.Scoped);
container.Register<CategoryGrpcService>(Lifestyle.Scoped);

builder.Services.AddScoped<AuthorGrpcService>(sp => container.GetInstance<AuthorGrpcService>());
builder.Services.AddScoped<BookGrpcService>(sp => container.GetInstance<BookGrpcService>());
builder.Services.AddScoped<BorrowingGrpcService>(sp => container.GetInstance<BorrowingGrpcService>());
builder.Services.AddScoped<CategoryGrpcService>(sp => container.GetInstance<CategoryGrpcService>());

// TODO: cors?

var app = builder.Build();

app.Services.UseSimpleInjector(container);
app.MapGrpcService<AuthorGrpcService>();
app.MapGrpcService<BookGrpcService>();
app.MapGrpcService<BorrowingGrpcService>();
app.MapGrpcService<CategoryGrpcService>();

// TODO: Only in test enviroment

container.Verify();

app.MapGet("/", () => "Hello World!");

app.Run();
public partial class Program { }