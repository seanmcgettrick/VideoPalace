using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VideoPalace.Catalog.Service.Data;
using VideoPalace.Catalog.Service.Entities;
using VideoPalace.Common.Extensions;
using VideoPalace.Common.Settings;

var builder = WebApplication.CreateBuilder(args);

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransitRabbitMq(builder.Configuration, serviceSettings!.ServiceName);

builder.Services
    .AddMongoDb(builder.Configuration, serviceSettings!.ServiceName)
    .AddMongoRepository<Movie>("movies");

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<SeedMovieCatalog>();

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