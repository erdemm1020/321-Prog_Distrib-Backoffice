using System;
using System.IO;
using System.Text.Json;

namespace Backoffice;

public class Time
{
    // Fuseau utilisé partout (UTC+1). Pour changer : modifier ce chiffre.
    public static readonly TimeSpan Fuseau = TimeSpan.FromHours(1);

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

    public void Save()
    {
        Directory.CreateDirectory(JsonSer.dataFolder);
        string path = Path.Combine(JsonSer.dataFolder, "time.json");

        string json = JsonSerializer.Serialize(this, JsonSer.JsonOptions);
        File.WriteAllText(path, json);
    }

    public DateTime LocalTime =>
        DateTimeOffset.FromUnixTimeSeconds(UnixTimeStamp).ToOffset(Fuseau).DateTime;

    public override string ToString() =>
        DateTimeOffset.FromUnixTimeSeconds(UnixTimeStamp).ToOffset(Fuseau).ToString("dd/MM/yyyy HH:mm:ss");
}