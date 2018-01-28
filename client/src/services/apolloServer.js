import axios from "axios";
import config from "../config";
import loginService from "./loginService";
import { metaActions } from "../redux/actions";
import { getStore } from "../redux";

var requestCounter = 0;

const invokeFull = async (commandName, payload) => {
  if (loginService.isLoggedIn()) {
    payload.token = loginService.getToken();
  }
  const store = getStore();

  try {
    store.dispatch(metaActions.incrementRequests());
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
    console.log("ERROR talking to server", err);
    throw err;
  } finally {
    store.dispatch(metaActions.decrementRequests());
  }
  result.data.status = result.status;
  return result.data;
};

const invoke = async (commandName, payload) => {
  var response = await invokeFull(commandName, payload);
  if (response.status !== 200) {
    console.warn("Server Error", response);
    throw Error("Bad Request");
  }
  return response.result.Result;
};

export default {
  invoke,
  invokeFull
};
