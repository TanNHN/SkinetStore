using System.Text.Json;
using API.Middlewares;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.SeedData;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 1. how about builder.Services.AddScoped<ProductRepository>(); ???
// A: DEPEND ON ABSTRACTION NOT CONCRETION, if create like that you have to inject a concrete class instead of an interface
// 2. vid 20, dependency injection, <interface, class implemented> 
//when class call this defined interface (eg:ProductController(IGenericRepository repo))
// an instance of GenericRepository will be create and pass to 'repo'

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString = builder.Configuration.GetConnectionString("Redis");
    if(string.IsNullOrEmpty(connString)) throw new Exception("Cannot get Redis connection string");
    ConfigurationOptions configurations = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configurations);
});
// BC Redis service is Singleton => tbis cart using redis has to be Singleton
builder.Services.AddSingleton<ICartService, CartService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.MapControllers();

//tự tạo db nếu ko có và sau đó gen seed data
try
{

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    // MigrateAsync, nếu ko có db sẽ thì tự tạo + appy any pending migration
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
app.Run();
