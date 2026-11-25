using System;
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

        // logs d'abord, pour ne rien rater
        await _client.SubscribeAsync(Logs.Topic);
        Console.WriteLine($"[MQTT] Abonné au topic {Logs.Topic}");

        await _client.SubscribeAsync(_topic);
        Console.WriteLine($"[MQTT] Abonné au topic {_topic}");
    }

    private Task OnMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        string json = e.ApplicationMessage.ConvertPayloadToString();
        Console.WriteLine($"[MQTT] Reçu sur {e.ApplicationMessage.Topic} : {json}");

        // un log n'est pas une heure : on l'enregistre et c'est tout
        if (e.ApplicationMessage.Topic == Logs.Topic)
        {
            Logs.Save(json, "recus.log");
            return Task.CompletedTask;
        }

        if (!Contracts.TryReadTime(json, out Time? time, out string error))
        {
            Console.WriteLine($"[MQTT] Message ignoré : {error}");
            return Task.CompletedTask;
        }

        time!.Save();
        TimeReceived?.Invoke(time);

        return Task.CompletedTask;
    }

    // retourne true si le message est parti
    public async Task<bool> PublishAsync(string topic, string payload)
    {
        if (!_client.IsConnected) return false;

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        var resultat = await _client.PublishAsync(message);
        return resultat.IsSuccess;
    }

    public async ValueTask DisposeAsync()
    {
        if (_client.IsConnected)
            await _client.DisconnectAsync();
        _client.Dispose();
    }
}
