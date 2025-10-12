using AbiWebsite.Components;
using AbiWebsite.Data;
using AbiWebsite.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AbiDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddPushServiceClient(options => {
    options.PublicKey = builder.Configuration["PushService:PublicKey"];
    options.PrivateKey = builder.Configuration["PushService:PrivateKey"];
});
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    // Apply any pending migrations
    try {
        var context = services.GetRequiredService<AbiDbContext>();
        await context.Database.MigrateAsync();
    } catch (Exception ex) {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }

    // Send daily summary
    var timer = new Timer(async _ => {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<NotificationService>();
        await service.SendIntervalMottoSummaryAsync();
    }, null, TimeSpan.Zero, TimeSpan.FromHours(3));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
//app.MapHub<MottoNotificationHub>("/mottoNotificationHub");



app.Run();