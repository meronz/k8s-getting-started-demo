using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace SensorsAPI.Client;

public class SensorsService: IAsyncDisposable
{
    private readonly GrpcChannel _channel;
    private readonly Sensors.SensorsClient _client;

    public SensorsService(IConfiguration configuration)
    {
        var hostname = configuration.GetConnectionString("SensorsAPI");
        _channel = GrpcChannel.ForAddress(hostname);
        _client = new (_channel);
    }

    public async Task<(Sensor[] Sensors, uint TotalCount)> GetSensorsAsync(uint start, uint take)
    {
        var reply = await _client.GetSensorsAsync(new() { Start = start, Take = take});
        return (reply.Sensors.ToArray(), reply.TotalCount);
    }

    public ValueTask DisposeAsync()
    {
        _channel.Dispose();
        return ValueTask.CompletedTask;
    }
}