using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using StackExchange.Redis;

namespace SensorsAPI.Server;

public class LoadDataHelper
{
    private readonly ConnectionMultiplexer _redis;
    
    public LoadDataHelper(ConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    private static readonly List<dynamic> MockData = new()
    {
        new { Id = 1,  Name = "Buffalo", Lat = "42.88", Lon = "-78.83", LastSeenOnline = 1632312655},
        new { Id = 2,  Name = "Kirovskaya", Lat = "55.5834553", Lon = "38.1546279", LastSeenOnline = 1608536545},
        new { Id = 3,  Name = "Kokofata", Lat = "12.8818852", Lon = "-9.9451106", LastSeenOnline = 1626805820},
        new { Id = 4,  Name = "Kréstena", Lat = "37.5923591", Lon = "21.6206833", LastSeenOnline = 1641388253},
        new { Id = 5,  Name = "Tonoshō", Lat = "34.5057425", Lon = "134.2087936", LastSeenOnline = 1594427495},
        new { Id = 6,  Name = "Ush-Tyube", Lat = "45.2415442", Lon = "77.9726477", LastSeenOnline = 1626282620},
        new { Id = 7,  Name = "San Martin", Lat = "19.5042619", Lon = "-98.8797029", LastSeenOnline = 1618200951},
        new { Id = 8,  Name = "Dancheng", Lat = "33.644743", Lon = "115.177188", LastSeenOnline = 1631740005},
        new { Id = 9,  Name = "Riti", Lat = "7.906903", Lon = "9.660661", LastSeenOnline = 1620816489},
        new { Id = 10, Name = "Taesal-li", Lat = "36.9714", Lon = "126.4542", LastSeenOnline = 1641823906},
        new { Id = 11, Name = "Alquízar", Lat = "22.8064234", Lon = "-82.5848464", LastSeenOnline = 1608495929},
        new { Id = 12, Name = "Moryń", Lat = "52.85768", Lon = "14.39293", LastSeenOnline = 1641159706},
        new { Id = 13, Name = "Sandovo", Lat = "58.4633132", Lon = "36.4116086", LastSeenOnline = 1613427234},
        new { Id = 14, Name = "Ibaan", Lat = "13.816877", Lon = "121.131952", LastSeenOnline = 1648952305},
        new { Id = 15, Name = "Desamparados", Lat = "9.899259", Lon = "-84.0624815", LastSeenOnline = 1606442918},
        new { Id = 16, Name = "Jeonju", Lat = "35.8242238", Lon = "127.1479532", LastSeenOnline = 1640217016},
        new { Id = 17, Name = "Dongsheng", Lat = "39.822507", Lon = "109.963338", LastSeenOnline = 1605570318},
        new { Id = 18, Name = "Gornji Breg", Lat = "45.8829678", Lon = "19.9675163", LastSeenOnline = 1605303769},
        new { Id = 19, Name = "Agidel’", Lat = "55.9005444", Lon = "53.9334354", LastSeenOnline = 1653747027},
        new { Id = 20, Name = "Francisco Beltrão", Lat = "-26.0779448", Lon = "-53.05199", LastSeenOnline = 1644304327},
        new { Id = 21, Name = "Yanglin", Lat = "27.955261", Lon = "112.497228", LastSeenOnline = 1649127267},
        new { Id = 22, Name = "Nassarawa", Lat = "8.5705151", Lon = "8.3088441", LastSeenOnline = 1600414536},
        new { Id = 23, Name = "Alamar", Lat = "23.1635991", Lon = "-82.2854848", LastSeenOnline = 1606602851},
        new { Id = 24, Name = "Trollhättan", Lat = "58.261515", Lon = "12.2964905", LastSeenOnline = 1649698399},
        new { Id = 25, Name = "Novi Bečej", Lat = "45.6011009", Lon = "20.1415393", LastSeenOnline = 1626435779},
        new { Id = 26, Name = "Zhaoguli", Lat = "39.1921745", Lon = "117.2630206", LastSeenOnline = 1632565541},
        new { Id = 27, Name = "Weizhou", Lat = "21.042256", Lon = "109.118014", LastSeenOnline = 1626932174},
        new { Id = 28, Name = "Vetlanda", Lat = "57.4346086", Lon = "15.0586931", LastSeenOnline = 1611834429},
        new { Id = 29, Name = "Shanjiang", Lat = "21.270702", Lon = "110.359368", LastSeenOnline = 1607401399},
        new { Id = 30, Name = "Stroitel’", Lat = "52.6523079", Lon = "41.4346698", LastSeenOnline = 1598083014},
        new { Id = 31, Name = "Yacimiento Río Turbio", Lat = "-51.57321", Lon = "-72.3508", LastSeenOnline = 1618543145},
        new { Id = 32, Name = "Bełk", Lat = "50.5504089", Lon = "20.418024", LastSeenOnline = 1602418416},
        new { Id = 33, Name = "Venilale", Lat = "-8.6417702", Lon = "126.3795987", LastSeenOnline = 1646935786},
        new { Id = 34, Name = "Irákleion", Lat = "35.3387006", Lon = "25.1443698", LastSeenOnline = 1596389923},
        new { Id = 35, Name = "Toledo", Lat = "41.6560291", Lon = "-83.666758", LastSeenOnline = 1603895829},
        new { Id = 36, Name = "Butou", Lat = "40.657978", Lon = "109.840313", LastSeenOnline = 1599490792},
        new { Id = 37, Name = "Karangturi", Lat = "-6.9895044", Lon = "110.4329118", LastSeenOnline = 1611259550},
        new { Id = 38, Name = "Jaquimeyes", Lat = "18.31173", Lon = "-71.16145", LastSeenOnline = 1650475418},
        new { Id = 39, Name = "Mikhaylovsk", Lat = "56.4428126", Lon = "59.129251", LastSeenOnline = 1652618560},
        new { Id = 40, Name = "Skwierzyna", Lat = "52.5967759", Lon = "15.5140181", LastSeenOnline = 1651298883},
        new { Id = 41, Name = "Schœlcher", Lat = "14.7525179", Lon = "-61.1759005", LastSeenOnline = 1628170454},
        new { Id = 42, Name = "Medang", Lat = "-6.267562", Lon = "106.6143944", LastSeenOnline = 1603982727},
        new { Id = 43, Name = "Sankt Andrä-Höch", Lat = "46.791555", Lon = "15.379192", LastSeenOnline = 1614893162},
        new { Id = 44, Name = "Yŏnmu", Lat = "36.12944", Lon = "127.1", LastSeenOnline = 1640234072},
        new { Id = 45, Name = "Gnieżdżewo", Lat = "54.7381652", Lon = "18.5728399", LastSeenOnline = 1623840199},
        new { Id = 46, Name = "Krynki", Lat = "53.2639896", Lon = "23.7725", LastSeenOnline = 1641924241},
        new { Id = 47, Name = "Paris La Défense", Lat = "48.892701", Lon = "2.233089", LastSeenOnline = 1624248588},
        new { Id = 48, Name = "Sambogunung", Lat = "-6.9915372", Lon = "112.5021593", LastSeenOnline = 1636013686},
        new { Id = 49, Name = "Puerto Ayacucho", Lat = "5.6614718", Lon = "-67.5827744", LastSeenOnline = 1594162320},
        new { Id = 50, Name = "Huomachong", Lat = "27.874777", Lon = "110.241387", LastSeenOnline = 1611718737}
    };

    public async Task Load()
    {
        const string key = "sensors";

        var db = _redis.GetDatabase();
        var count = await db.ListLengthAsync(key);
        //await db.KeyDeleteAsync(key);
        if (count == 0)
        {
            foreach (var el in MockData)
            {
                var sensor = new Sensor()
                {
                    Id = el.Id,
                    Name = el.Name,
                    Lat = el.Lat,
                    Lon = el.Lon,
                    LastSeenOnline = Timestamp.FromDateTime(DateTime.UnixEpoch.AddSeconds(el.LastSeenOnline)),
                };
                var json = JsonSerializer.Serialize(sensor);
                if(json is null) throw new("Invalid data!");
                db.ListRightPush(key, json);
            }
        }
    }
}