import axios from 'axios';
import config from '../config';
import loginService from './login-service';

var requestCounter = 0;

module.exports = {
  invoke: function(commandName, payload) {

    if (loginService.isLoggedIn()) {
      payload.token = loginService.getToken();
    }
    //console.log(`COMMAND ${commandName}`, payload);
    return axios.post(config.apiUrl + 'api', {
      id: (requestCounter++).toString(),
      method: commandName,
      params: payload
    }, {
      validateStatus: function(status) {
        return true;
      }
    }).then(response => {
      //console.log("RESPONSE SUCCESS", response);
      if (response.data.error === "Unauthorized Command") {
        loginService.logout();
        window.location.reload();
        return [];
      }

      return response.data.result.Result;
    }).catch((err) => {
      console.log("ERROR", err);
      throw err;
    });
  },
  invokeFull: function(commandName, payload) {

    if (loginService.isLoggedIn()) {
      payload.token = loginService.getToken();
    }
    //console.log(`COMMAND ${commandName}`, payload);
    return axios.post(config.apiUrl + 'api', {
      id: (requestCounter++).toString(),
      method: commandName,
      params: payload
    }, {
      validateStatus: function(status) {
        return true;
      }
    }).then(response => {
      //console.log("RESPONSE SUCCESS", response);
      if (response.data.error === "Unauthorized Command") {
        loginService.logout();
        window.location.reload();
        return [];
      }

      return response.data;
    }).catch((err) => {
      console.log("ERROR", err);
      throw err;
    });
  },
};
