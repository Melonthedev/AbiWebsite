using AbiWebsite.Components;
using AbiWebsite.Data;
using AbiWebsite.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AbiDbContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection"), 
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options => {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(300);
    });
builder.Services.Configure((AbiWebsite.Models.HostOptions options) => builder.Configuration.Bind("Host", options));
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
/*builder.Services.AddPushServiceClient(options => {
    options.Subject = builder.Configuration["PushService:Subject"];
    options.PublicKey = builder.Configuration["PushService:PublicKey"];
    options.PrivateKey = builder.Configuration["PushService:PrivateKey"];
    options.DefaultAuthenticationScheme = Lib.Net.Http.WebPush.Authentication.VapidAuthenticationScheme.Vapid;
    options.Expiration = 60 * 60;
});*/
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddControllers();
var app = builder.Build();

// Apply any pending migrations
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try {
        var context = services.GetRequiredService<AbiDbContext>();
        await context.Database.MigrateAsync();
    } catch (Exception ex) {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

var timer = new Timer(async _ => {
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Send Push-Notification...");
    var service = scope.ServiceProvider.GetRequiredService<NotificationService>();
    await service.SendIntervalMottoSummaryAsync();
}, null, TimeSpan.FromHours(3), TimeSpan.FromHours(3)); // alle 3 Stunden

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();