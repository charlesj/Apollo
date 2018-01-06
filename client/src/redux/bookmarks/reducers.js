import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  bookmarks: {},
  total: 0
};

export default handleActions(
  {
    [combineActions(
      actions.load.start,
      actions.save.start,
      actions.remove.start
    )]: basicStartReducer,

    [combineActions(
      actions.load.fail,
      actions.save.fail,
      actions.remove.fail
    )]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const { total, bookmarks } = action.payload;
      const current = { ...state.bookmarks };
      bookmarks.forEach(bookmark => {
        current[bookmark.id] = bookmark;
      });

      return {
        ...basicLoadCompleteReducer(state, action),
        total,
        bookmarks: current
      };
    },

    [actions.save.complete]: (state, action) => {
      const { bookmark } = action.payload;
      const current = { ...state.bookmarks };
      current[bookmark.id] = bookmark;

      return {
        ...basicLoadCompleteReducer(state, action),
        bookmarks: current
      };
    },

    [actions.remove.complete]: (state, action) => {
      const { bookmark } = action.payload;
      const current = { ...state.bookmarks };
      delete current[bookmark.id];

      return {
        ...basicLoadCompleteReducer(state, action),
        bookmarks: current
      };
    }
  },
  initialState
);
