using System.Text.Json;
using System.Text.Json.Serialization;
using Hippocampus.Api;
using Hippocampus.Domain.Configurations.AutoMapper;
using Hippocampus.Domain.Models.Entities;
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

builder
    .Services.AddSingleton<IClock>(new Clock())
    .AddDbContext<HippocampusContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddAutoMapper(config => config.AddProfile(typeof(AutoMapperProfile)));

builder
    .Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddTransient<IWaterTankRepository, WaterTankRepository>();
builder.Services.AddTransient<IWaterTankService, WaterTankService>();
builder.Services.AddTransient<IWaterTankLogRepository, WaterTankLogRepository>();
builder.Services.AddTransient<IWaterTankLogService, WaterTankLogService>();
builder.Services.AddTransient<IWaterTankStateUpdateProcessor, WaterTankStateUpdateProcessor>();

var app = builder.Build();

// await app.MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader().AllowAnyMethod());

var api = app.MapGroup("api");
api.MapMonitorRoutes();
api.MapLogRoutes();

app.Run();
