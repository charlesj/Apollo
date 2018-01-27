import _ from "lodash";
import { keyedIdToArray } from "../selector-helpers";

export function feeds(state) {
  return keyedIdToArray(state.feeds.feeds);
}

export function currentFeed(state) {
  return state.feeds.currentFeed;
}

export function currentItem(state) {
  return state.feeds.currentItem;
}

export function items(state) {
  const { selectedFeed } = state.feeds;
  let items = keyedIdToArray(state.feeds.items);
  if (selectedFeed) {
    items = items.filter(item => item.feed_id === selectedFeed.id);
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
    next = allItems.filter(item => item.published_at > current.published_at);
  } else {
    previous = [];
    next = allItems;
  }
  return {
    previousItem: _.last(previous),
    previous,
    nextItem: _.first(next),
    next,
    current
  };
}
