using AbiWebsite.Data;
using AbiWebsite.Models;
using Lib.Net.Http.WebPush;
using System.Diagnostics;

namespace AbiWebsite.Services {
    public class NotificationService(AbiDbContext db, PushServiceClient pushClient) {
        private readonly AbiDbContext _db = db;
        private readonly PushServiceClient _pushClient = pushClient;

        public async Task AddSuggestionAsync(MottoSuggestion suggestion) {
            _db.MottoSuggestions.Add(suggestion);
            await _db.SaveChangesAsync();

            /*var subscriptions = _db.PushSubscriptions.ToList();
            var payload = $"{{\"title\":\"Neuer Mottovorschlag\",\"body\":\"{suggestion.Title}: {suggestion.Description}\"}}";

            foreach (var sub in subscriptions) {
                var pushSubscription = new Lib.Net.Http.WebPush.PushSubscription {
                    Endpoint = sub.Endpoint,
                    Keys = new Dictionary<string, string> {
                        ["p256dh"] = sub.P256DH,
                        ["auth"] = sub.Auth
                    }
                };

                var message = new PushMessage(payload) {
                    Topic = "Neue Motto-Vorschlag",
                    Urgency = PushMessageUrgency.Normal
                };

                try {
                    await _pushClient.RequestPushMessageDeliveryAsync(pushSubscription, message);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }*/
        }

        public async Task SendDailyMottoSummaryAsync() {
            var today = DateTime.UtcNow.Date;
            var mottos = _db.MottoSuggestions
                .Where(m => m.CreatedAt.Date == today)
                .ToList();

            int count = mottos.Count;
            if (count == 0)
                return;

            // Titel: "n neue Mottovorschläge!"
            var title = $"{count} neue Mottovorschläge!";
            // Die ersten 3 Vorschläge auflisten
            var listed = mottos.Take(3)
                .Select(m => $"- {m.Title}{(string.IsNullOrWhiteSpace(m.Description) ? "" : ": " + m.Description)}");
            var description = string.Join("\n", listed);
            if (count > 3)
                description += $"\n...und {count - 3} weitere";

            // Link zur Ranking-Seite
            var url = "/mottoranking";

            // Payload mit Link
            var payload = $"{{\"title\":\"{title}\",\"body\":\"{description}\",\"url\":\"{url}\"}}";

            var subscriptions = _db.PushSubscriptions.ToList();
            foreach (var sub in subscriptions) {
                var pushSubscription = new Lib.Net.Http.WebPush.PushSubscription {
                    Endpoint = sub.Endpoint,
                    Keys = new Dictionary<string, string> {
                        ["p256dh"] = sub.P256DH,
                        ["auth"] = sub.Auth
                    }
                };

                var message = new PushMessage(payload) {
                    Topic = "Motto-Tageszusammenfassung",
                    Urgency = PushMessageUrgency.Normal
                };

                try {
                    await _pushClient.RequestPushMessageDeliveryAsync(pushSubscription, message);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }
        }

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
            var payload = $"{{\"title\":\"{title}\",\"body\":\"{description}\",\"url\":\"{url}\"}}";

            var subscriptions = _db.PushSubscriptions.ToList();
            foreach (var sub in subscriptions) {
                var pushSubscription = new Lib.Net.Http.WebPush.PushSubscription {
                    Endpoint = sub.Endpoint,
                    Keys = new Dictionary<string, string> {
                        ["p256dh"] = sub.P256DH,
                        ["auth"] = sub.Auth
                    }
                };

                var message = new PushMessage(payload) {
                    Topic = "Motto-Zusammenfassung",
                    Urgency = PushMessageUrgency.Normal
                };

                try {
                    await _pushClient.RequestPushMessageDeliveryAsync(pushSubscription, message);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}