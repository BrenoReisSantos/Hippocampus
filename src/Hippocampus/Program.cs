using System.Text.Json;
using System.Text.Json.Serialization;
using Hippocampus.Api;
using Hippocampus.Domain.Models.Context;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<IClock>(new Clock())
    .AddDbContext<HippocampusContext>(
        options => options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new MacAddressJsonConverter());
    options.SerializerOptions.Converters.Add(new RecipientLevelJsonConverter());
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = false;
}).Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new MacAddressJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new RecipientLevelJsonConverter());
});

var app = builder.Build();

// await app.MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var api = app.MapGroup("api");
api.MapLogRoutes();

app.Run();