using EfUnitTesting.Data;
using EfUnitTesting.Data.UnitOfWork;
using EfUnitTesting.QueryFilter.Tenants;
using EfUnitTesting.Repositories;
using EfUnitTesting.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => 
{
    setup.OperationFilter<TenantHeaderSwaggerAttribute>();
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IBatchGenreService, BatchGenreService>();
builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
builder.Services.AddScoped<TenantService>();

builder.Services.AddDbContext<MoviesContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
       .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
}, ServiceLifetime.Scoped, ServiceLifetime.Singleton);


// Check the performance and then apply this
//builder.Services.AddDbContextPool<MoviesContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//       .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
//});

var app = builder.Build();

// DIRTY HACK, we WILL come back to fix this
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
