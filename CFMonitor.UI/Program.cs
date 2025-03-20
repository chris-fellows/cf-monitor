using CFMonitor.Interfaces;
using CFMonitor.Services;
using CFMonitor.UI.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");

// Add data services
builder.Services.AddScoped<IFileObjectService>((scope) =>
{
    return new XmlFileObjectService(Path.Combine(configFolder, "FileObject"));
});
builder.Services.AddScoped<IMonitorAgentService>((scope) =>
{
    return new XmlMonitorAgentService(Path.Combine(configFolder, "MonitorAgent"));
});
builder.Services.AddScoped<IMonitorItemService>((scope) =>
{
    return new XmlMonitorItemService(Path.Combine(configFolder, "MonitorItem"));
});
builder.Services.AddScoped<ISystemValueTypeService>((scope) =>
{
    return new XmlSystemValueTypeService(Path.Combine(configFolder, "SystemValueType"));
});
builder.Services.AddScoped<IUserService>((scope) =>
{
    return new XmlUserService(Path.Combine(configFolder, "User"));
});
builder.Services.AddScoped<IMonitorItemTypeService, MonitorItemTypeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
