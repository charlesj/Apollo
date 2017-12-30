import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import moment from "moment";
import apolloServer from "../../services/apolloServer";
import loginService from "../../services/loginService";

const actionCreators = createActions({
  meta: {
    login: basicActions(),
    logout: basicActions(),
    notify: basicActions(),
    toggleNotificationRead: basicActions()
  }
});

const actions = actionCreators.meta;

export default actions;

export function notify({ type, message }) {
  console.log("action", type, message);
  return dispatchBasicActions(actions.notify, () => {
    return { type, message, unread: true, time: moment() };
  });
}

export function toggleNotificationRead({ type, message, unread, time }) {
  return dispatchBasicActions(actions.toggleNotificationRead, () => {
    return { type, message, unread: !unread, time };
  });
}

export function login(password) {
  return dispatchBasicActions(actions.login, async () => {
    const loginResult = await apolloServer.invoke("Login", {
      password
    });

    if (loginResult.token) {
      return { token: loginResult.token };
    } else {
      return { loginError: true };
    }
  });
}

export function logout() {
  return dispatchBasicActions(actions.logout, async () => {
    var token = loginService.getToken();
    await apolloServer.invoke("revokeLoginSession", {
      tokenToRevoke: token
    });
    return {
      result: true
    };
  });
}
