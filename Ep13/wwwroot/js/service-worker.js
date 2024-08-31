// service-worker.js
self.addEventListener('push', function(event) {
    const data = event.data.json();
    const options = {
      body: data.body,
      icon: data.icon || 'icon.png', // URL to the icon
      badge: data.badge || 'badge.png', // URL to the badge icon
      data: data.url || '/', // URL to open when the notification is clicked
    };
  console.log(data);
   // Show notification
   event.waitUntil(
    self.registration.showNotification(data.title || 'Default Title', options)
      .then(() => console.log('Notification shown successfully'))
      .catch(err => console.error('Error showing notification:', err))
  );
});

self.addEventListener('notificationclick', function(event) {
  console.log('Notification click received:', event.notification);

  event.notification.close(); // Close the notification

  // Open the URL in a new window/tab
  if (event.notification.data) {
    event.waitUntil(
      clients.openWindow(event.notification.data)
        .then(windowClient => console.log('Window opened:', windowClient))
        .catch(err => console.error('Error opening window:', err))
    );
  }
});