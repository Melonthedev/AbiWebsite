window.pushInterop = {
    registerServiceWorker: async function () {
        if ('serviceWorker' in navigator) {
            await navigator.serviceWorker.register('/service-worker.js');
            console.log('ServiceWorker registered!');
        }
    },
    subscribeToPush: async function (publicKey) {
        console.log("Attempting to subscribe to push notifications with public key " + publicKey + " ...");
        const registration = await navigator.serviceWorker.ready;
        console.log("Service Worker ready!");

        if (typeof Notification === "undefined") {
            console.log("Push-API nicht unterstützt (Notification ist undefined).");
            return "unsupported";
        }

        // Permission explizit anfragen, falls noch nicht gesetzt
        if (Notification.permission === "default") {
            try {
                const permission = await Notification.requestPermission();
                if (permission !== "granted") {
                    console.log("Push permission denied by user.");
                    return "denied";
                }
            } catch (err) {
                console.log("Error requesting notification permission:", err);
                return "error";
            }
        } else if (Notification.permission === "denied") {
            console.log("Push permission denied by user.");
            return "denied";
        }

        // Prüfe, ob bereits eine Subscription existiert
        let sub = await registration.pushManager.getSubscription();
        if (!sub) {
            try {
                sub = await registration.pushManager.subscribe({
                    userVisibleOnly: true,
                    applicationServerKey: urlBase64ToUint8Array(publicKey)
                });
                console.log("Subscribed serviceworker, sending request to backend...");
                // Sende die Subscription an das Backend
                var response = await fetch('/api/push/subscribe', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        endpoint: sub.endpoint,
                        p256dh: sub.getKey('p256dh') ? btoa(String.fromCharCode.apply(null, new Uint8Array(sub.getKey('p256dh')))) : null,
                        auth: sub.getKey('auth') ? btoa(String.fromCharCode.apply(null, new Uint8Array(sub.getKey('auth')))) : null
                    })
                });
                console.log('Push subscription sent to server.');
                console.log("Response status:", response.status);
                const text = await response.text();
                console.log("Response body:", text);
            } catch (err) {
                console.log("Push subscription failed:", err);
                return "error";
            }
        } else {
            console.log("Push subscription already exists, not subscribing again.");
        }
        return "granted";
    },
    getNotificationPermission: function () {
        return localStorage.getItem("notificationPermission");
    },
    setNotificationPermission: function (value) {
        localStorage.setItem("notificationPermission", value);
    },

};

function urlBase64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');
    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);
    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

