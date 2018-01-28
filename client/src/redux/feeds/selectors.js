import _ from "lodash";
import { loadItems } from "./actions";
import { keyedIdToArray } from "../selector-helpers";
import { getDispatch } from "../";

let throttleLoadItems = false;

export function feeds(state) {
  const feeds = keyedIdToArray(state.feeds.feeds);

  return _.orderBy(feeds, ['name']).filter(f => f.unread_count > 0);
}

export function currentFeed(state) {
  return state.feeds.currentFeed;
}

export function currentItem(state) {
  return state.feeds.currentItem;
}

export function items(state) {
  const { currentFeed } = state.feeds;
  let items = keyedIdToArray(state.feeds.items);
  if (currentFeed) {
    items = items.filter(item => item.feed_id === currentFeed.id);
  }

  return _.orderBy(items, "published_at", "asc");
}

export function displayItems(state) {
  const allItems = items(state);
  const current = currentItem(state);
  let previous, next;
  if (current) {
    previous = allItems.filter(
      item => item.published_at < current.published_at
    );
    if(previous.length > 5){
      previous = previous.slice(Math.max(previous.length - 5, 1))
    }
    next = allItems.filter(item => item.published_at > current.published_at);
  } else {
    previous = [];
    next = allItems;
  }

  if (next.length < 5 && !throttleLoadItems) {
    throttleLoadItems = true;
    const { currentFeed } = state.feeds;
    const dispatch = getDispatch();
    const feedId = currentFeed ? currentFeed.id : -1;
    dispatch(loadItems(feedId));
    setTimeout(() => {
      throttleLoadItems = false;
    }, 30000);
  }

  return {
    previousItem: _.last(previous),
    previous,
    nextItem: _.first(next),
    next,
    current
  };
}
