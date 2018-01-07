import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  boards: {},
  boardItems: {}
};

export default handleActions(
  {
    [combineActions(
      actions.load.start,
      actions.saveBoard.start,
      actions.moveBoard.start,
      actions.removeBoard.start,
      actions.loadItems.start,
      actions.saveItem.start,
      actions.removeItem.start
    )]: basicStartReducer,

    [combineActions(
      actions.load.fail,
      actions.saveBoard.fail,
      actions.moveBoard.fail,
      actions.removeBoard.fail,
      actions.loadItems.fail,
      actions.saveItem.fail,
      actions.removeItem.fail
    )]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const { boards } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        boards: idReducer(state.boards, boards)
      };
    },

    [actions.saveBoard.complete]: (state, action) => {
      const board = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        boards: idReducer(state.boards, board)
      };
    },

    [actions.moveBoard.complete]: (state, action) => {
      const { boards } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        boards: idReducer(state.boards, boards)
      };
    },

    [actions.removeBoard.complete]: (state, action) => {
      const { board } = action.payload;
      const updated = { ...state.boards };
      delete updated[board.id];

      return {
        ...basicLoadCompleteReducer(state, action),
        boards: updated
      };
    },

    [actions.loadItems.complete]: (state, action) => {
      const { items } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        boardItems: idReducer(state.boardItems, items)
      };
    },

    [actions.saveItem.complete]: (state, action) => {
      const { item } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        boardItems: idReducer(state.boardItems, item)
      };
    },

    [actions.removeItem.complete]: (state, action) => {
      const { id } = action.payload;
      const updated = { ...state.boardItems };
      delete updated[id];

      return {
        ...basicLoadCompleteReducer(state, action),
        boardItems: updated
      };
    }
  },
  initialState
);
