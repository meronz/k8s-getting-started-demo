namespace BlazorCharts.Shared;

public record Sensor
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Lat { get; set; } = "";
    public string Lon { get; set; } = "";
    public DateTime LastSeenOnline { get; set; }
}