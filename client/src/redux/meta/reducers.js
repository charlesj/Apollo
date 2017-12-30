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
  notifications: []
};

export default handleActions(
  {
    [combineActions(
      actions.login.start,
      actions.logout.start,
      actions.notify.start
    )]: basicStartReducer,

    [combineActions(
      actions.login.fail,
      actions.logout.fail,
      actions.notify.fail
    )]: basicFailReducer,

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

    [actions.toggleNotificationRead.complete]: (state, action) => {
      const updated = action.payload;
      const notifications = { ...state.notifications };
      notifications.forEach(n => {
        if (n.time === updated.time && n.message === updated.message) {
          n.unread = updated.unread;
        }
      });

      return {
        ...basicLoadCompleteReducer(state, action),
        notifications
      };
    }
  },
  initialState
);
