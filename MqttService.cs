using System;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet;

namespace Backoffice;

public class MqttService : IAsyncDisposable
{
    private readonly IMqttClient _client;
    private readonly string _host;
    private readonly int _port;
    private readonly string _topic;

    public event Action<Time>? TimeReceived;

    public MqttService(string host, int port, string topic)
    {
        _host = host;
        _port = port;
        _topic = topic;
        _client = new MqttClientFactory().CreateMqttClient();
        _client.ApplicationMessageReceivedAsync += OnMessage;
    }

    public async Task StartAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_host, _port)
            .WithClientId("erdem")
            .WithCredentials("ict", "321")
            .Build();

        await _client.ConnectAsync(options);
        Console.WriteLine($"[MQTT] Connecté à {_host}:{_port}");

        await _client.SubscribeAsync(_topic);
        Console.WriteLine($"[MQTT] Abonné au topic {_topic}");
    }

    private Task OnMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        string json = e.ApplicationMessage.ConvertPayloadToString();
        Console.WriteLine($"[MQTT] Reçu sur {e.ApplicationMessage.Topic} : {json}");

        try
        {
            Time? time = JsonSerializer.Deserialize<Time>(json, JsonSer.JsonOptions);

            if (time == null)
            {
                Console.WriteLine("[MQTT] JSON vide, ignoré.");
                return Task.CompletedTask;
            }

            time.Save();

            TimeReceived?.Invoke(time);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[MQTT] JSON invalide : {ex.Message}");
        }

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_client.IsConnected)
            await _client.DisconnectAsync();
        _client.Dispose();
    }
}
