namespace VideoPalace.Common.Settings;

public class MongoDbSettings
{
    public string Host { get; init; } = default!;
    public int Port { get; init; }

    public string ConnectionString => $"mongodb://{Host}:{Port}";
}