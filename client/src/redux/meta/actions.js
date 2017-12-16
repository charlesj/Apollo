import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";
import loginService from "../../services/loginService";

const actionCreators = createActions({
  meta: {
    login: basicActions(),
    logout: basicActions()
  }
});

const actions = actionCreators.meta;

export default actions;

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
