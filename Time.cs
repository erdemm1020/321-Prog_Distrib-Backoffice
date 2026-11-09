using System;
using System.IO;
using System.Text.Json;

namespace Backoffice;

public class Time
{
    public long UnixTimeStamp { get; set; }

    public Time(long unixTimeStamp)
    {
        UnixTimeStamp = unixTimeStamp;
    }

    public static Time? LoadTime()
    {
        string path = Path.Combine(JsonSer.dataFolder, "time.json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Time>(json, JsonSer.JsonOptions);
    }

    public override string ToString() =>
        DateTimeOffset.FromUnixTimeSeconds(UnixTimeStamp)
                      .ToLocalTime()
                      .ToString("dd/MM/yyyy HH:mm:ss");
}