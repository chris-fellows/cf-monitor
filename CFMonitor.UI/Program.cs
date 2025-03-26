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
using System;
using CFUtilities.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//var valueAsString = new ValueAsString();
//var booleanValue = true;
//valueAsString.SetValue(booleanValue);
//var newBooleanValue = valueAsString.GetValue();
//var type1 = newBooleanValue.GetType();

//var dateTime = DateTimeOffset.UtcNow;
//valueAsString.SetValue(dateTime);
//var newDateTime = valueAsString.GetValue();
//var type2 = newDateTime.GetType();

//var newValue = valueAsString.GetValue();

// CMF Added
// Set authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "auth_cookie";
            options.LoginPath = "/login";
            options.Cookie.MaxAge = TimeSpan.FromMinutes(120);
            options.AccessDeniedPath = "/access-denied";
        });

builder.Services.AddAuthorization();            // CMF Added
builder.Services.AddCascadingAuthenticationState();     // CMF Added

builder.Services.AddHttpContextAccessor();  // Added for IRequestContextService

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
builder.Services.AddScoped<IMonitorAgentGroupService>((scope) =>
{
    return new XmlMonitorAgentGroupService(Path.Combine(configFolder, "MonitorAgentGroup"));
});
builder.Services.AddScoped<IMonitorItemOutputService>((scope) =>
{
    return new XmlMonitorItemOutputService(Path.Combine(configFolder, "MonitorItemOutput"));
});
builder.Services.AddScoped<IMonitorItemService>((scope) =>
{
    return new XmlMonitorItemService(Path.Combine(configFolder, "MonitorItem"));
});
builder.Services.AddScoped<IMonitorItemCheckService>((scope) =>     // Only used by Monitor Agent
{
    return new XmlMonitorItemCheckService(Path.Combine(configFolder, "MonitorItemCheck"));  
});
builder.Services.AddScoped<IPasswordResetService>((scope) =>
{
    return new XmlPasswordResetService(Path.Combine(configFolder, "PasswordReset"));
});
builder.Services.AddScoped<ISystemValueTypeService>((scope) =>
{
    return new XmlSystemValueTypeService(Path.Combine(configFolder, "SystemValueType"));
});
builder.Services.AddScoped<IUserService>((scope) =>
{
    return new XmlUserService(Path.Combine(configFolder, "User"), scope.GetRequiredService<IPasswordService>());
});

// Add request context service
builder.Services.AddScoped<IRequestContextService, RequestContextService>();

// Add memory cache
builder.Services.AddSingleton<ICache, MemoryCache>();

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

// Add toast service
builder.Services.AddSingleton<IToastService, ToastService>();

// Add placeholder service. E.g. Replacing error message placeholder in email body
builder.Services.AddScoped<IPlaceholderService, PlaceholderService>();

// Add password service
builder.Services.AddScoped<IPasswordService, PBKDF2PasswordService>();

// Seed
builder.Services.AddKeyedScoped<IEntityReader<ActionItemType>, ActionItemTypeSeed1>("ActionItemTypeSeed");
builder.Services.AddKeyedScoped<IEntityReader<AuditEventType>, AuditEventTypeSeed1>("AuditEventTypeSeed");
builder.Services.AddKeyedScoped<IEntityReader<EventItem>, EventItemSeed1>("EventItemSeed");
builder.Services.AddKeyedScoped<IEntityReader<FileObject>, FileObjectSeed1>("FileObjectSeed");
builder.Services.AddKeyedScoped<IEntityReader<MonitorAgentGroup>, MonitorAgentGroupSeed1>("MonitorAgentGroupSeed");
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
app.UseAuthentication();    // CMF Added
app.UseAuthorization();     // CMF Added
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
