using SensorsAPI.Server;
using SensorsAPI.Server.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add the gRPC services
builder.Services.AddGrpc();

// Redis services
builder.Services.AddSingleton<LoadDataHelper>();
builder.Services.AddSingleton<ConnectionMultiplexer>(s =>
{
    var conf = s.GetRequiredService<IConfiguration>();
    return ConnectionMultiplexer.Connect(conf.GetConnectionString("Redis"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SensorsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// Load initial data in Redis
await app.Services.GetRequiredService<LoadDataHelper>().Load();

app.Run();
