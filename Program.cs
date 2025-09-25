using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TvShowTrackerDbContext>(options => options.UseSqlite("Data Source=tvshowtracker.db"));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<TvShowUpdateService>();

var app = builder.Build();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<TvShowTrackerDbContext>();

// reset the database on each run
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

app.Run();
