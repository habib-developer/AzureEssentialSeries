if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/js/service-worker.js')
      .then(function(registration) {
        console.log('Service Worker registered with scope:', registration.scope);
      })
      .catch(function(error) {
        console.error('Service Worker registration failed:', error);
      });
  }

  
  if ('Notification' in window && 'serviceWorker' in navigator) {
    Notification.requestPermission().then(permission => {
      if (permission === 'granted') {
        console.log('Notification permission granted.');
        subscribeUserToPush();
      } else {
        console.log('Notification permission denied.');
      }
    });
  }
  
  function subscribeUserToPush() {
    navigator.serviceWorker.ready.then(function(registration) {
      const vapidPublicKey = 'VAPID Public key'; // Replace with your VAPID public key
      const convertedVapidKey = urlBase64ToUint8Array(vapidPublicKey);
  
      registration.pushManager.subscribe({
        userVisibleOnly: true,
        applicationServerKey: convertedVapidKey
      })
      .then(function(subscription) {
        console.log('User is subscribed:', subscription);
  
        // Send subscription to the server
        sendSubscriptionToServer(subscription);
      })
      .catch(function(error) {
        console.error('Failed to subscribe the user:', error);
      });
    });
  }
  
  function urlBase64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - (base64String.length % 4)) % 4);
    const base64 = (base64String + padding).replace(/-/g, '+').replace(/_/g, '/');
    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);
  
    for (let i = 0; i < rawData.length; ++i) {
      outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
  }

  function sendSubscriptionToServer(subscription) {
    // Extract p256dh and auth keys from subscription
    const keys = {
      endpoint: subscription.endpoint,
      keys: {
        p256dh: btoa(String.fromCharCode.apply(
          null, new Uint8Array(subscription.getKey('p256dh'))
        )),
        auth: btoa(String.fromCharCode.apply(
          null, new Uint8Array(subscription.getKey('auth'))
        ))
      }
    };

    console.log('Subscription Keys:', keys);
    // return fetch('/api/save-subscription', {
    //   method: 'POST',
    //   body: JSON.stringify(subscription),
    //   headers: {
    //     'Content-Type': 'application/json'
    //   }
    // })
    // .then(function(response) {
    //   if (!response.ok) {
    //     throw new Error('Failed to save subscription on server.');
    //   }
    //   return response.json();
    // })
    // .then(function(data) {
    //   console.log('Subscription saved on server:', data);
    // })
    // .catch(function(error) {
    //   console.error('Failed to save subscription on server:', error);
    // });
  }
  