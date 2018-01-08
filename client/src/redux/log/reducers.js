import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  entries: {},
  total: 0
};

export default handleActions(
  {
    [combineActions(actions.load.start, actions.save.start)]: basicStartReducer,

    [combineActions(actions.load.fail, actions.save.fail)]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const { total, entries } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        entries: idReducer(state.entries, entries),
        total
      };
    },

    [actions.save.complete]: (state, action) => {
      const { entry } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        entries: idReducer(state.entries, entry),
        total: state.total + 1
      };
    }
  },
  initialState
);
