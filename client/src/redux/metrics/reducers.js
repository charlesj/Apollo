import { combineActions, handleActions } from "redux-actions";
import _ from "lodash";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  metrics: []
};

export default handleActions(
  {
    [combineActions(actions.loadMetrics.start)]: basicStartReducer,

    [combineActions(actions.loadMetrics.fail)]: basicFailReducer,

    [actions.loadMetrics.complete]: (state, action) => {
      const { metrics } = action.payload;
      const newMetrics = _.unionBy(metrics, state.metrics.metrics, "id");
      return {
        ...basicLoadCompleteReducer(state, action),
        metrics: newMetrics
      };
    }
  },
  initialState
);
