using CFMonitor.UI.Data;
using CFMonitor.EntityReader;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Seed;
using CFMonitor.Services;
using CFMonitor.UI.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;
using CFUtilities.Interfaces;
using CFUtilities.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Set config folder
var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");
//Directory.Delete(configFolder, true);

// Add data services
builder.Services.AddScoped<IActionItemTypeService>((scope) =>
{
    return new XmlActionItemTypeService(Path.Combine(configFolder, "ActionItemType"));
});
builder.Services.AddScoped<IAuditEventService>((scope) =>
{
    return new XmlAuditEventService(Path.Combine(configFolder, "AuditEvent"));
});
builder.Services.AddScoped<IAuditEventTypeService>((scope) =>
{
    return new XmlAuditEventTypeService(Path.Combine(configFolder, "AuditEventType"));
});
builder.Services.AddScoped<IEventItemService>((scope) =>
{
    return new XmlEventItemService(Path.Combine(configFolder, "EventItem"));
});
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

// Add monitor item type service (Static data)
builder.Services.AddScoped<IMonitorItemTypeService, MonitorItemTypeService>();

// Add system value display service
builder.Services.AddScoped<ISystemValueDisplayService, SystemValueDisplayService>();

// Add audit event factory
builder.Services.AddScoped<IAuditEventFactory, AuditEventFactory>();

// Add file security checker (E.g. Checking images being uploaded)
builder.Services.AddScoped<IFileSecurityCheckerService, FileSecurityCheckerService>();

// Add event item factory
builder.Services.AddScoped<IEventItemFactoryService, EventItemFactoryService>();

// Add placeholder service. E.g. Replacing error message placeholder in email body
builder.Services.AddScoped<IPlaceholderService, PlaceholderService>();

// Add password service
builder.Services.AddScoped<IPasswordService, PBKDF2PasswordService>();

// Seed
builder.Services.AddKeyedScoped<IEntityReader<ActionItemType>, ActionItemTypeSeed1>("ActionItemTypeSeed");
builder.Services.AddKeyedScoped<IEntityReader<AuditEventType>, AuditEventTypeSeed1>("AuditEventTypeSeed");
builder.Services.AddKeyedScoped<IEntityReader<EventItem>, EventItemSeed1>("EventItemSeed");
builder.Services.AddKeyedScoped<IEntityReader<MonitorAgent>, MonitorAgentSeed1>("MonitorAgentSeed");
builder.Services.AddKeyedScoped<IEntityReader<MonitorItem>, MonitorItemSeed1>("MonitorItemSeed");
builder.Services.AddKeyedScoped<IEntityReader<SystemValueType>, SystemValueTypeSeed1>("SystemValueTypeSeed");
builder.Services.AddKeyedScoped<IEntityReader<User>, UserSeed1>("UserSeed");

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

//// Load data
//using (var scope = app.Services.CreateScope())
//{
//    //// Check for data
//    //var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();
//    //var systemValuesTypes = systemValueTypeService.GetAll();
//    //if (!systemValuesTypes.Any())
//    //{
//    //    throw new ArgumentException("System contains no data");
//    //}

//    // Enable this to load seed data
//    //new SeedLoader().DeleteAsync(scope).Wait();
//    new SeedLoader().LoadAsync(scope).Wait();
//}

app.Run();
