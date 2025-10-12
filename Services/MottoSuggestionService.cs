using AbiWebsite.Data;
using AbiWebsite.Models;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace AbiWebsite.Services {
    public class MottoSuggestionService(AbiDbContext db, PushServiceClient pushClient) {
        private readonly AbiDbContext _db = db;
        //private readonly IHubContext<MottoNotificationHub> _hubContext = hubContext;
        private readonly PushServiceClient _pushClient = pushClient;

        public async Task AddSuggestionAsync(MottoSuggestion suggestion) {
            _db.MottoSuggestions.Add(suggestion);
            await _db.SaveChangesAsync();

            // SignalR Notification an alle Clients
            //await _hubContext.Clients.All.SendAsync("ReceiveMottoSuggestion", suggestion.Title, suggestion.Description);

            // Web Push an alle Abonnenten
            var subscriptions = _db.PushSubscriptions.ToList();
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
            }
        }
    }
}