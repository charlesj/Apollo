import Ember from 'ember';

export default Ember.Route.extend({
  apollo: Ember.inject.service('apollo-server'),
  model: function(){
    return this.get('apollo').request('ApplicationInfo', {});
  }
});
