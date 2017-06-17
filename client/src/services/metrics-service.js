var apolloServer = require('./apollo-server');

var addMetric = function(category, name, value) {
  return apolloServer.invoke('addMetric', {
    category: category,
    name: name,
    value: value
  });
}

var getMetrics = function(category, name) {
  return apolloServer.invoke('getMetrics', {
    category: category,
    name: name
  });
}

module.exports = {
  addMetric: addMetric,
  getMetrics: getMetrics
}
