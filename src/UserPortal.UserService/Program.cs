using Microsoft.EntityFrameworkCore;
using UserPortal.UserService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var defaultConnection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Production") ?? false
  ? Environment.GetEnvironmentVariable("DefaultConnection")
  : builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(defaultConnection))
{
  throw new ArgumentNullException(nameof(defaultConnection), $"The DefaultConnection must be passed in Environment Variables");
}

builder.Services.AddDbContext<UserDbContext>(opt =>
{
  opt.UseNpgsql(defaultConnection, sql =>
  {
    sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
    sql.MigrationsHistoryTable("__EFMigrationsHistory", "user");
  });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
  var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
  context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
