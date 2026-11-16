using System;
using System.IO;
using System.Text.Json.Nodes;
using Json.Schema;

namespace Backoffice;

// Valide les messages MQTT avec les mêmes schémas que les tests (dossier contrats)
public static class Contracts
{
    private static readonly string Dir = Path.Combine(AppContext.BaseDirectory, "contrats");

    private static readonly JsonSchema EnvelopeSchema = JsonSchema.FromFile(Path.Combine(Dir, "envelope.schema.json"));
    private static readonly JsonSchema TimeSchema = JsonSchema.FromFile(Path.Combine(Dir, "time.schema.json"));

    // Accepte uniquement une enveloppe de type "time" valide ; le simple {"UnixTimeStamp": ...} est refusé
    public static bool TryReadTime(string json, out Time? time, out string error)
    {
        time = null;
        error = "";

        JsonNode? message;
        try
        {
            message = JsonNode.Parse(json);
        }
        catch (Exception ex)
        {
            error = $"JSON invalide : {ex.Message}";
            return false;
        }

        // "format" (uuid, date-time) n'est qu'une annotation sans RequireFormatValidation
        var options = new EvaluationOptions { RequireFormatValidation = true };

        if (!EnvelopeSchema.Evaluate(message, options).IsValid)
        {
            error = "ne respecte pas envelope.schema.json";
            return false;
        }

        if (message!["type"]?.GetValue<string>() != "time")
        {
            error = "enveloppe qui n'est pas de type time";
            return false;
        }

        JsonNode? payload = message["payload"];
        if (!TimeSchema.Evaluate(payload, options).IsValid)
        {
            error = "payload ne respecte pas time.schema.json";
            return false;
        }

        time = new Time(payload!["timestamp"]!.GetValue<long>());
        return true;
    }
}
