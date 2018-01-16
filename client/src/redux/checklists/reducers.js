import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  checklists: [],
  selectedChecklist: null,
  checklistItems: [],
  completedChecklists: [],
  completionLog: []
};

export default handleActions(
  {
    [combineActions(
      actions.getChecklists.start,
      actions.saveChecklist.start,
      actions.removeChecklist.start,
      actions.addCompletedChecklist.start,
      actions.getChecklistCompletionLog.start
    )]: basicStartReducer,

    [combineActions(
      actions.getChecklists.fail,
      actions.saveChecklist.fail,
      actions.removeChecklist.fail,
      actions.addCompletedChecklist.fail,
      actions.getChecklistCompletionLog.fail
    )]: basicFailReducer,

    [actions.getChecklists.complete]: (state, action) => {
      const { checklists } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        checklists: idReducer(state.checklists, checklists)
      };
    },

    [actions.selectChecklist]: (state, action) => {
      return {
        ...state,
        selectedChecklist: action.payload
      };
    },

    [actions.saveChecklist.complete]: (state, action) => {
      const { checklist } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        checklists: idReducer(state.checklists, checklist),
        selectedChecklist: null
      };
    },

    [actions.removeChecklist.complete]: (state, action) => {
      const { checklist } = action.payload;
      const updated = { ...state.checklists };
      delete updated[checklist.id];
      return {
        ...basicLoadCompleteReducer(state, action),
        checklists: updated,
        selectedChecklist: null
      };
    },

    [actions.addCompletedChecklist.complete]: (state, action) => {
      const { completedChecklist } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        completedChecklists: idReducer(
          state.completedChecklists,
          completedChecklist
        )
      };
    },

    [actions.getChecklistCompletionLog.complete]: (state, action) => {
      const { checklistLog } = action.payload;

      const updated = { ...state.completionLog };
      checklistLog.forEach(obj => {
        updated[obj.completion_id] = obj;
      });
      return {
        ...basicLoadCompleteReducer(state, action),
        completionLog: updated
      };
    }
  },
  initialState
);
