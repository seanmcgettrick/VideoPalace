using System.Net.Http.Headers;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using VideoPalace.Catalog.Service.Data;
using VideoPalace.Catalog.Service.Entities;
using VideoPalace.Catalog.Service.Services;
using VideoPalace.Common.Extensions;
using VideoPalace.Common.Settings;

const string inventoryServiceUrl = "InventoryServiceUrl";

var builder = WebApplication.CreateBuilder(args);

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMongoDb(builder.Configuration, serviceSettings!.ServiceName)
    .AddMongoRepository<Movie>("movies");

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<SeedMovieCatalog>();

builder.Services.AddHttpClient<IInventoryService, InventoryService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration[inventoryServiceUrl]!);
});

    var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();