using Hippocampus.Domain.Configurations.AutoMapper;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<HippocampusContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services
    .AddSingleton<IClock>(new Clock());
builder.Services.AddAutoMapper(config =>
    config.AddProfile(typeof(AutoMapperProfile)));

builder.Services.AddMudServices();
builder.Services.AddTransient<IRecipientMonitorServices, RecipientMonitorServices>();
builder.Services.AddTransient<IRecipientMonitorRepository, RecipientMonitorRepository>();
builder.Services.AddTransient<IRecipientLogRepository, RecipientLogRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();