using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using StackExchange.Redis;

namespace SensorsAPI.Server.Services;

public class SensorsService : Sensors.SensorsBase
{
    private readonly ILogger<SensorsService> _logger;
    private readonly ConnectionMultiplexer _redis;

    public SensorsService(ILogger<SensorsService> logger, ConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public override async Task<SensorsReply> GetSensors(SensorsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Called!");
        
        var db = _redis.GetDatabase();
        var totalCount = await db.ListLengthAsync("sensors");
        var redisValues = await db.ListRangeAsync("sensors", request.Start, request.Start + request.Take);
        var values = redisValues
            .Select(_ => JsonSerializer.Deserialize<Sensor>(_.ToString()))
            .Where(deserialized => deserialized is not null)
            .ToList();

        var reply = new SensorsReply();
        reply.Sensors.AddRange(values);
        reply.TotalCount = (uint)totalCount;
        return reply;
    }
}
