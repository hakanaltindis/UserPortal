using UserPortal.ManagementService;
using UserPortal.ManagementService.Consumers;
using UserPortal.ManagementService.Data;
using UserPortal.ManagementService.Services;
using UserPortal.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.RegisterDbContext<ManagementDbContext>(builder.Configuration);

builder.Services.RegisterRabbitMQ(builder.Configuration, typeof(UserServiceConsumer).Assembly);

builder.Services.RegisterBussinessServices(typeof(Program).Assembly, typeof(Program).Assembly);

builder.Services.AddAutoMapper(typeof(ManagementServiceMapperProfile));

var app = builder.Build();

app.ApplyMigration<ManagementDbContext>();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
