import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer
} from "../redux-helpers";
import actions from "./actions";

const initialState = {
  goals: []
};

function NormalizeDateTimeOffsetToDate(datetimeoffsetstring) {
  if (datetimeoffsetstring.length > 0) {
    return datetimeoffsetstring.split("T")[0];
  }
}

export default handleActions(
  {
    [combineActions(
      actions.getGoals.start,
      actions.upsertGoal.start
    )]: basicStartReducer,

    [combineActions(
      actions.getGoals.fail,
      actions.upsertGoal.fail
    )]: basicFailReducer,

    [actions.getGoals.complete]: (state, action) => {
      const { goals } = action.payload;
      goals.forEach(g => {
        g.startDate = NormalizeDateTimeOffsetToDate(g.startDate);
        g.endDate = NormalizeDateTimeOffsetToDate(g.endDate);
      });
      return {
        ...basicLoadCompleteReducer(state, action),
        goals
      };
    },

    [actions.upsertGoal.complete]: (state, action) => {
      const { goal } = action.payload;
      const goals = state.goals.map(g => {
        if (g.Id === goal.Id) {
          return goal;
        } else {
          return g;
        }
      });

      if (
        goals.filter(g => {
          return g.Id === goal.Id;
        }).length === 0
      ) {
        goals.push(goal);
      }

      return {
        ...basicLoadCompleteReducer(state, action),
        goals
      };
    }
  },
  initialState
);
