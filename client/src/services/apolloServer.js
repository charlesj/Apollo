import axios from "axios";
import config from "../config";
import loginService from "./loginService";
import { metaActions } from "../redux/actions";
import store from "../redux";

var requestCounter = 0;

const invokeFull = async (commandName, payload) => {
  if (loginService.isLoggedIn()) {
    payload.token = loginService.getToken();
  }

  store.dispatch(metaActions.incrementRequests());
  try {
    var result = await axios.post(
      config.apiUrl + "api",
      {
        id: (requestCounter++).toString(),
        method: commandName,
        params: payload
      },
      {
        validateStatus: function(status) {
          return true;
        }
      }
    );

    if (result.data.error === "Unauthorized Command") {
      loginService.logout();
      window.location.reload();
      return [];
    }
  } catch (err) {
    throw err;
  } finally {
    store.dispatch(metaActions.decrementRequests());
  }

  return result.data;
};

const invoke = async (commandName, payload) => {
  var response = await invokeFull(commandName, payload);
  return response.result.Result;
};

export default {
  invoke,
  invokeFull
};
