import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  notes: []
};

export default handleActions(
  {
    [combineActions(
      actions.getAll.start,
      actions.saveNote.start
    )]: basicStartReducer,

    [combineActions(
      actions.getAll.fail,
      actions.saveNote.fail
    )]: basicFailReducer,

    [actions.getAll.complete]: (state, action) => {
      const { notes } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        notes: idReducer(state.notes, notes)
      };
    },

    [actions.saveNote.complete]: (state, action) => {
      const { note } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        notes: idReducer(state.notes, note)
      };
    }
  },
  initialState
);
