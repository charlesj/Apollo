import axios from "axios";
import config from "../config";
import loginService from "./loginService";

var requestCounter = 0;

const invokeFull = async (commandName, payload) => {
  if (loginService.isLoggedIn()) {
    payload.token = loginService.getToken();
  }
  //console.log(`COMMAND ${commandName}`, payload);
  return axios
    .post(
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
    )
    .then(response => {
      //console.log("RESPONSE SUCCESS", response);
      if (response.data.error === "Unauthorized Command") {
        loginService.logout();
        window.location.reload();
        return [];
      }

      return response.data;
    })
    .catch(err => {
      console.log("ERROR", err);
      throw err;
    });
};

const invoke = async (commandName, payload) => {
  var response = await invokeFull(commandName, payload);
  return response.result.Result;
};

export default {
  invoke,
  invokeFull
};
