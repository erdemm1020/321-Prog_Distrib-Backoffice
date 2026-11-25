using System;
using System.IO;

namespace Backoffice;

public static class Logs
{
    private static readonly object Verrou = new();

    // 2026-11-23T10:14:02+01:00 [info] COM-0000 (umddcmid3b/log) message
    public static string Format(long unixTimeStamp, string niveau, string? commerceId, string message)
    {
        string date = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp)
            .ToOffset(Time.Fuseau).ToString("yyyy-MM-dd'T'HH:mm:sszzz");
        return $"{date} [{niveau}] {commerceId} ({Topics.Log}) {message}";
    }

    // Ajoute la ligne dans un fichier du dossier Data
    // backoffice.log = logs envoyés, recus.log = logs reçus par MQTT
    public static void Save(string ligne, string fichier = "backoffice.log")
    {
        lock (Verrou)
        {
            Directory.CreateDirectory(JsonSer.dataFolder);
            File.AppendAllText(Path.Combine(JsonSer.dataFolder, fichier), ligne + Environment.NewLine);
        }
    }
}
