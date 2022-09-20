using System.Text.RegularExpressions;
using AutoMapper;

namespace BlazorCharts.Server;

public class SensorsAPIMapper: Profile
{
    /// <summary>
    /// Creates an AutoMapper profile between
    /// BlazorCharts.Shared and SensorsAPI objects
    /// </summary>
    public SensorsAPIMapper()
    {
        CreateMap<SensorsAPI.Sensor, Sensor>()
            .ForMember(
                dst => dst.LastSeenOnline,
                o => o.MapFrom(src => src.LastSeenOnline.ToDateTime()));
    }
}