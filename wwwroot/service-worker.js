self.addEventListener('push', function (event) {
    console.log("Received Push Event:");
    const data = event.data.json();
    console.log(data);
    const options = {
        body: data.body,
        icon: '/media/cropped-cropped-Logoelement_WFS_tuerkis-1-1-1-192x192.png', // Pfad zu deinem Icon
        badge: '/media/cropped-cropped-Logoelement_WFS_tuerkis-1-1-1-192x192.png', // Optional: kleines Icon für die Statusleiste
        data: { url: data.url }
    };
    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

self.addEventListener('notificationclick', function(event) {
    event.notification.close();
    const url = event.notification.data.url || '/';
    event.waitUntil(
        clients.openWindow(url)
    );
});