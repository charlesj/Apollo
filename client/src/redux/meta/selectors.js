import _ from "lodash";

export function unreadNotificationCount(state) {
  return state.meta.notifications.filter(n => {
    return n.unread;
  }).length;
}

export function latestNotification(state) {
  return _.last(_.orderBy(state.meta.notifications, ["time"], ["asc"]));
}

export function activeServerRequests(state) {
  return state.meta.activeRequests > 0;
}
