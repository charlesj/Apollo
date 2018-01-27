import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import { actions } from "./actions";

const initialState = {
  feeds: {},
  items: {},
  currentItem: null,
  currentFeed: null
};

export default handleActions(
  {
    [combineActions(
      actions.loadList.start,
      actions.loadItems.start,
      actions.setCurrentItem.start
    )]: basicStartReducer,

    [combineActions(
      actions.loadList.fail,
      actions.loadItems.fail,
      actions.setCurrentItem.fail
    )]: basicFailReducer,

    [actions.loadList.complete]: (state, action) => {
      const feeds = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        feeds: idReducer(state.feeds, feeds)
      };
    },

    [actions.loadItems.complete]: (state, action) => {
      const { items } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        items: idReducer(state.items, items)
      };
    },

    [actions.setCurrentItem.complete]: (state, action) => {
      const { item } = action.payload;
      const newFeeds = { ...state.feeds };
      const newItems = idReducer(state.items, item);
      if (!item.read_at) {
        item.read_at = new Date();
        newFeeds[item.feed_id].unread_count =
          newFeeds[item.feed_id].unread_count - 1;
      }

      return {
        ...basicLoadCompleteReducer(state, action),
        feeds: newFeeds,
        items: newItems,
        currentItem: item
      };
    },

    [actions.selectFeed.complete]: (state, action) => {
      const { feed, items } = action.payload;
      return {
        ...state,
        items: idReducer(state.items, items),
        currentFeed: feed
      };
    }
  },
  initialState
);
