using System.Threading.Channels;

namespace BlazorCharts.Server.Hubs;

public class TemperatureHub : Hub
{
    private readonly ILogger<TemperatureHub> _logger;
    private readonly Random _random = new(51122113);

    public TemperatureHub(ILogger<TemperatureHub> logger)
    {
        _logger = logger;
    }

    public ChannelReader<TemperatureData> LiveData()
    {
        var channel = Channel.CreateUnbounded<TemperatureData>();

        _ = HandleLiveData(channel.Writer);

        return channel.Reader;
    }

    private async Task HandleLiveData(ChannelWriter<TemperatureData> outStream)
    {
        var lastSample = new TemperatureData(DateTime.Now, 20);
        do
        {
            await Task.Delay(500);
            var delta = _random.NextDouble();
            var newTemperature = _random.NextDouble() >= 0.5 ? lastSample.Temperature + delta : lastSample.Temperature - delta;
            lastSample = new (DateTime.Now, newTemperature);
            _logger.LogInformation("Sending sample {lastSample}", lastSample);
        }
        while (!Context.ConnectionAborted.IsCancellationRequested && outStream.TryWrite(lastSample));
        
        outStream.Complete();
        _logger.LogInformation("Channel complete");
    }
}