import apolloServer from "../../services/apolloServer";

var getFeeds = () => {
  return apolloServer.invoke("getFeeds", {});
};

var getItems = feedId => {
  return apolloServer.invoke("getFeedItems", {
    feedId
  });
};

var markItemAsRead = itemId => {
  return apolloServer.invoke("markItemAsRead", {
    itemId
  });
};

export default {
  getFeeds,
  getItems,
  markItemAsRead
};
