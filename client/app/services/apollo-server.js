import Ember from 'ember';

export default Ember.Service.extend({
  ajax: Ember.inject.service(),

  request(commandName, payload) {
    console.log(`COMMAND ${commandName}`);
    console.log(payload);
    return this.get('ajax').request('http://localhost:8042', {
      method: 'POST',
      contentType: "application/json",
      data:JSON.stringify({
        id: 'whatever',
        method: commandName,
        params: payload
      })
    }).then(response => {
      console.log("RESPONSE SUCCESS");
      console.log(response);
      return response.result.Result;
    }).catch(err =>{
      console.log("ERROR");
      console.log(err);
    });
  }
});
