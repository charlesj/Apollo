import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer
} from "../redux-helpers";
import actions from "./actions";
import loginService from "../../services/loginService";

const initialState = {
  token: loginService.getToken(),
  notifications: [],
  activeRequests: 0
};

export default handleActions(
  {
    [combineActions(
      actions.login.start,
      actions.logout.start
    )]: basicStartReducer,

    [combineActions(actions.login.fail, actions.logout.fail)]: basicFailReducer,

    [actions.login.complete]: (state, action) => {
      const { token, loginError } = action.payload;

      if (token && token.length > 1) {
        loginService.storeToken(token);
      }

      return {
        ...basicLoadCompleteReducer(state, action),
        token,
        loginError
      };
    },

    [actions.logout.complete]: (state, action) => {
      loginService.logout();
      return {
        ...basicLoadCompleteReducer(state, action),
        token: null,
        loginError: null
      };
    },

    [actions.notify.complete]: (state, action) => {
      const notifications = [...state.notifications];
      notifications.push(action.payload);
      return {
        ...basicLoadCompleteReducer(state, action),
        notifications
      };
    },

    [actions.toggleNotificationRead]: (state, action) => {
      const updated = action.payload;
      const notifications = [...state.notifications];
      notifications.forEach(n => {
        if (n.time === updated.time && n.message === updated.message) {
          n.unread = updated.unread;
        }
      });

      return {
        ...basicLoadCompleteReducer(state, action),
        notifications
      };
    },

    [actions.dismissNotification]: (state, action) => {
      const toRemove = action.payload;
      const notifications = state.notifications.filter(n => {
        return n.time !== toRemove.time;
      });

      return {
        ...basicLoadCompleteReducer(state, action),
        notifications
      };
    },

    [actions.markAllNotificationsRead]: (state, action) => {
      const notifications = state.notifications.map(n => {
        return {
          ...n,
          unread: false
        };
      });

      return {
        ...state,
        notifications
      };
    },

    [actions.dismissAllNotifications]: (state, action) => {
      return {
        ...state,
        notifications: []
      };
    },

    [actions.incrementRequests]: (state, action) => {
      return {
        ...state,
        activeRequests: state.activeRequests + 1
      };
    },

    [actions.decrementRequests]: (state, action) => {
      return {
        ...state,
        activeRequests: state.activeRequests - 1
      };
    }
  },
  initialState
);
