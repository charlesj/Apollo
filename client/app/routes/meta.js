import Ember from 'ember';

export default Ember.Route.extend({
  apollo: Ember.inject.service('apollo-server'),
  actions:{
    test(){
      this.get('apollo').request('ApplicationInfo', {})
      .then(appInfo =>{
        console.log(appInfo.commitHash);
      });
    }
  }
});
