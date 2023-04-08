using System.Text.Json;
using System.Text.Json.Serialization;
using Hippocampus.Api;
using Hippocampus.Domain.Configurations.AutoMapper;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services
    .AddSingleton<IClock>(new Clock())
    .AddDbContext<HippocampusContext>(
        options => options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.AddAutoMapper(config =>
    config.AddProfile(typeof(AutoMapperProfile)));

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

builder.Services.AddTransient<IRecipientMonitorRepository, RecipientMonitorMonitorRepository>();
builder.Services.AddTransient<IRecipientMonitorServices, RecipientMonitorServices>();

var app = builder.Build();

// await app.MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader());

var api = app.MapGroup("api");
api.MapLogRoutes();

app.Run();