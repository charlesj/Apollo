import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  feeds: {
    loadList: basicActions(),
    //addFeed: basicActions(),
    loadItems: basicActions(),
    setCurrentItem: basicActions(),
    selectFeed: basicActions()
  }
});

export const actions = actionCreators.feeds;

export function loadList() {
  return dispatchBasicActions(actions.loadList, async () => {
    const feeds = await apolloServer.invoke("getFeeds", {});
    return feeds;
  });
}

export function loadItems(feedId) {
  return dispatchBasicActions(actions.loadItems, async () => {
    const items = await apolloServer.invoke("getFeedItems", { feedId });
    return { items };
  });
}

export function setCurrentItem(item) {
  return dispatchBasicActions(actions.setCurrentItem, async () => {
    if (!item.read_at) {
      await apolloServer.invoke("markItemAsRead", { itemId: item.id });
    }

    return { item };
  });
}

export function selectFeed(feed) {
  return dispatchBasicActions(actions.selectFeed, async () => {
    let items = [];
    if (feed) {
      items = await apolloServer.invoke("getFeedItems", { feedId: feed.id });
    }

    return { feed, items };
  });
}
