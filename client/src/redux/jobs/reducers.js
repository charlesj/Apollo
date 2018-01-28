import { combineActions, handleActions } from "redux-actions";
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer
} from "../redux-helpers";
import { actions } from "./actions";

const initialState = {
  jobs: {},
  jobHistories: {},
  selectedJob: null
};

export default handleActions(
  {
    [combineActions(
      actions.load.start,
      actions.selectJob.start
    )]: basicStartReducer,

    [combineActions(
      actions.load.fail,
      actions.selectJob.fail
    )]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const { jobs } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        jobs: idReducer(state.jobs, jobs)
      };
    },

    [actions.selectJob.complete]: (state, action) => {
      const { jobHistories, job } = action.payload;

      return {
        ...basicLoadCompleteReducer(state, action),
        selectedJob: job,
        jobHistories: idReducer(state.jobHistories, jobHistories)
      };
    }
  },
  initialState
);
