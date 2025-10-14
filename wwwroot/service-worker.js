self.addEventListener('push', function (event) {
    console.log("Received Push Event:");
    console.log(event);
    console.log(event.data);
    const data = event.data.json();
    console.log(data);
    const options = {
        body: data.body,
        icon: '/media/cropped-cropped-Logoelement_WFS_tuerkis-1-1-1-192x192.png', 
        badge: '/media/cropped-cropped-Logoelement_WFS_tuerkis-1-1-1-192x192.png', 
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

self.addEventListener('install', event => {
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    self.clients.claim();
});

// Optional: Caching für Offline-Fähigkeit
self.addEventListener('fetch', event => {
    event.respondWith(fetch(event.request));
});