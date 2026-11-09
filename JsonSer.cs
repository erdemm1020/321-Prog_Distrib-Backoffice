using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Backoffice;

public class JsonSer
{
    public static string _dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        // indente le josn sur plusieurs lignes pour qu'il soit lisible
        WriteIndented = true,
        // encode correctment avec les accents
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };


}
