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
  activeRequests: 0,
  applicationInfo: {}
};

export default handleActions(
  {
    [combineActions(
      actions.login.start,
      actions.logout.start,
      actions.applicationInfo.start
    )]: basicStartReducer,

    [combineActions(
      actions.login.fail,
      actions.logout.fail,
      actions.applicationInfo.fail
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

    [actions.applicationInfo.complete]: (state, action) => {
      const { applicationInfo } = action.payload;

      return {
        ...state,
        applicationInfo
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
