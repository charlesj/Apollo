var axios = require('axios');
var config = require('../config');

module.exports = {
  invoke: function(commandName, payload) {
    console.log(`COMMAND ${commandName}`);
    console.log(payload);
    return axios.post(config.apiUrl + 'api', {
      id: 'whatever',
      method: commandName,
      params: payload
    }).then(response => {
      console.log("RESPONSE SUCCESS");
      console.log(response);
      return response.data.result.Result;
    }).catch(err => {
      console.log("ERROR");
      console.log(err);
    });
  }
};
