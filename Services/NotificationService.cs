using AbiWebsite.Data;
using System.Text.Json;
using WebPush;

namespace AbiWebsite.Services {
    public class NotificationService(AbiDbContext db, ILogger<NotificationService> logger, IConfiguration config) {
        private readonly AbiDbContext _db = db;
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly IConfiguration _config = config;

        public async Task SendIntervalMottoSummaryAsync() {
            var now = DateTime.UtcNow;
            var intervalStart = now.AddHours(-3);
            var mottos = _db.MottoSuggestions
                .Where(m => m.CreatedAt >= intervalStart && m.CreatedAt <= now)
                .ToList();

            int count = mottos.Count;
            if (count == 0)
                return;

            var title = $"{count} neue Mottovorschläge!";
            if (count == 1) {
                title = "Neuer Mottovorschlag!";
            }
            var listed = mottos.Take(3)
                .Select(m => $"{m.Title}");
            var description = string.Join("\n", listed);
            if (count > 3)
                description += $"\n...und {count - 3} weitere";

            var url = "/mottoranking";
            var payloadObj = new { title, body = description, url };
            var payload = JsonSerializer.Serialize(payloadObj);

            var subscriptions = _db.PushSubscriptions.ToList();
            foreach (var sub in subscriptions) {
                _logger.LogInformation("Found Subscription: " + sub.Endpoint);

                var subject = _config["PushService:Subject"];
                var publicKey = _config["PushService:PublicKey"];
                var privateKey = _config["PushService:PrivateKey"];

                var pushsubscription = new PushSubscription(sub.Endpoint, sub.P256DH, sub.Auth);
                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var webPushClient = new WebPushClient();

                try {
                    await webPushClient.SendNotificationAsync(pushsubscription, payload, vapidDetails);
                    _logger.LogInformation($"Sent Push Notification! (P256DH {pushsubscription.P256DH})");
                } catch (Exception ex) {
                    _logger.LogWarning(ex.Message);
                }
            }
        }

        public async Task SendNotificationAsync(string title, string content, string url = "/mottoranking") {
            var payloadObj = new { title, body = content, url };
            var payload = JsonSerializer.Serialize(payloadObj);
            var subscriptions = _db.PushSubscriptions.ToList();
            foreach (var sub in subscriptions) {
                _logger.LogInformation("Found Subscription: " + sub.Endpoint);
                var subject = _config["PushService:Subject"];
                var publicKey = _config["PushService:PublicKey"];
                var privateKey = _config["PushService:PrivateKey"];
                var pushsubscription = new PushSubscription(sub.Endpoint, sub.P256DH, sub.Auth);
                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var webPushClient = new WebPushClient();
                try {
                    await webPushClient.SendNotificationAsync(pushsubscription, payload, vapidDetails);
                    _logger.LogInformation($"Sent Push Notification! (P256DH {pushsubscription.P256DH})");
                } catch (Exception ex) {
                    _logger.LogWarning(ex.Message);
                }
            }
        }
    }
}