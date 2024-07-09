using EfUnitTesting.Data;
using EfUnitTesting.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGenreRepository, GenreRepository>();

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
