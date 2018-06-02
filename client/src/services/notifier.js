export const NotifySuccess = (message, title) => {
  notify(message, title || 'Success')
}

export const NotifyError = (message, title) => {
  notify(message, title || 'Error')
}

function notify(message, title) {
  let notification
  if (!('Notification' in window)) {
    console.warn('Notifications are not supported')
  } else if (Notification.permission === 'granted') {
    notification = new Notification(title, {
      body: message,
      icon: '/favicon.ico',
    })
  } else if (Notification.permission !== 'denied') {
    Notification.requestPermission(function(permission) {
      if (permission === 'granted') {
        notification = new Notification(title, {
          body: message,
          icon: '/favicon.ico',
        })
      }
    })
  }
  if (notification) {
    setTimeout(notification.close.bind(notification), 5000)
  }
}
