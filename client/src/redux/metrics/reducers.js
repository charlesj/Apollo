import { combineActions, handleActions } from "redux-actions";
import _ from "lodash";
import moment from "moment";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer
} from "../redux-helpers";
import actions from "./actionCreators";

const initialState = {
  metrics: []
};

function normalizeMetrics(metric) {
  return {
    ...metric,
    created_at_moment: moment(metric.created_at),
    created_day: moment(metric.created_at).startOf("day")
  };
}

export default handleActions(
  {
    [combineActions(
      actions.loadMetrics.start,
      actions.addMetrics.start
    )]: basicStartReducer,

    [combineActions(
      actions.loadMetrics.fail,
      actions.addMetrics.fail
    )]: basicFailReducer,

    [actions.loadMetrics.complete]: (state, action) => {
      const { metrics } = action.payload;
      const newMetrics = _.unionBy(
        metrics.map(normalizeMetrics),
        state.metrics,
        "id"
      );
      return {
        ...basicLoadCompleteReducer(state, action),
        metrics: newMetrics
      };
    },

    [actions.addMetrics.complete]: (state, action) => {
      const metrics = action.payload
      const newMetrics = _.unionBy(
        metrics.map(normalizeMetrics),
        state.metrics,
        "id"
      );
      return {
        ...basicLoadCompleteReducer(state, action),
        metrics: newMetrics
      };
    }
  },
  initialState
);
