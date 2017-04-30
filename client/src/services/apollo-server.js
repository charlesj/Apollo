var axios = require('axios');
var config = require('../config');
var LoginService = require('./login-service');

var requestCounter = 0;

module.exports = {
  invoke: function(commandName, payload) {
    var loginService = new LoginService();
    if (loginService.isLoggedIn()) {
      payload.token = loginService.getToken();
    }

    console.log(`COMMAND ${commandName}`, payload);
    return axios.post(config.apiUrl + 'api', {
      id: (requestCounter++).toString(),
      method: commandName,
      params: payload
    }).then(response => {
      console.log("RESPONSE SUCCESS", response);
      return response.data.result.Result;
    }).catch(err => {
      console.log("ERROR", err);
    });
  }
};
