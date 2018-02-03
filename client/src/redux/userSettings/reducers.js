import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";

import { actions } from "./actions";

const initialState = {
  settings: {}
};

export default handleActions(
  {
    [combineActions(actions.load.start, actions.save.start)]: basicStartReducer,

    [combineActions(actions.load.fail, actions.save.fail)]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const settings = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        settings: idReducer(state.settings, settings)
      };
    },

    [actions.save.complete]: (state, action) => {
      const { setting } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        settings: idReducer(state.settings, setting)
      };
    },

    [actions.select]: (state, action) => {
      return {
        ...state,
        selectedSetting: action.payload
      };
    }
  },
  initialState
);
