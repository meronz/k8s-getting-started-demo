using AutoMapper;
using SensorsAPI.Client;

namespace BlazorCharts.Server.Hubs;

public class SensorsHub : Hub
{
    private readonly SensorsService _sensorsService;
    private readonly IMapper _mapper;
    private readonly ILogger<SensorsHub> _logger;

    public SensorsHub(SensorsService sensorsService, IMapper mapper, ILogger<SensorsHub> logger)
    {
        _sensorsService = sensorsService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Sensor[]> GetSensors(uint start, uint take)
    {
        try
        {
            _logger.LogInformation("Invoked");
            var reply = await _sensorsService.GetSensorsAsync(start, take);
            var mappedReply = _mapper.Map<Sensor[]>(reply.Sensors);
            return mappedReply;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
            throw;
        }
    }
}