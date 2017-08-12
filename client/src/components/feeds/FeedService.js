import apolloServer from '../../services/apollo-server';

var getFeeds = () => {
  return apolloServer.invoke('getFeeds', {});
}

var getItems = (feedId) => {
  return apolloServer.invoke('getFeedItems', {feedId});
}

var markItemAsRead = (itemId) => {
  return apolloServer.invoke('markItemAsRead', {itemId});
}

module.exports = {
  getFeeds,
  getItems,
  markItemAsRead
}
