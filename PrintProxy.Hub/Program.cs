using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PrintProxy.Hub.Components;
using PrintProxy.Hub.Components.Account;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Services;
using PrintProxy.Hub.Extensions;
using PrintProxy.Hub.Services.Files;
using PrintProxy.Hub.Services.reservation;
using PrintProxy.Hub.Services.Tags;
using MQTTnet.AspNetCore;
using MQTTnet;
using BitzArt.Blazor.Cookies;
using Serilog.AspNetCore;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

string AppdataPath = builder.Configuration.GetValue<string>("SERVER_APPDATA") ?? string.Empty;

Log.Logger = new LoggerConfiguration()
             .WriteTo.File(
                path: Path.Combine(AppdataPath,"ErrorLogs", "ErrorLog.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {Message}{NewLine}{Exception}"
             )
             .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
             .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database", Serilog.Events.LogEventLevel.Error)
             .CreateLogger();

builder.Services.AddSerilog();

// Add MudBlazor services
builder.Services.AddMudServices();

builder.AddBlazorCookies();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(a =>
{
    a.Configuration = builder.Configuration.GetConnectionString("RedisCache");
    a.InstanceName = "PrintCache";
});

builder.AddSelfSignedSslCert();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddControllers();

builder.Services.AddScoped<IPrinterThumbnailService,PrinterThumbnailService>();
builder.Services.AddScoped<IPrinterFactory, PrinterFactory>();
builder.Services.AddScoped<IPrinterConfigurationService, PrinterConfigurationService>();
builder.Services.AddScoped<IServerOptionsService, ServerOptionsService>();
builder.Services.AddScoped<IAppDataService, AppDataService>();

builder.Services.AddScoped<IPrinterIndexService, PrinterIndexService>();
builder.Services.AddScoped<IPrinterfileService, PrinterfileService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddSingleton<EmergencyManager>();
builder.Services.AddSingleton<MqttClientFactory>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddHealthChecks();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();



builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();
}

await app.SetupDefaultAdminUserAsync(); // First time setup

app.MapHealthChecks("/health");

using(var scope = app.Services.CreateScope())
{
    var indexer = scope.ServiceProvider.GetRequiredService<IPrinterIndexService>();
    await indexer.BeginIndexingAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);


app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
