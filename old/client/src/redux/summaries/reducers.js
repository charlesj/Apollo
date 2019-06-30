import { combineActions, handleActions, } from 'redux-actions'
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
} from '../redux-helpers'
import actions from './actions'

const initialState = {
  summaries: [],
}

export default handleActions(
  {
    [combineActions(actions.getSummaries.start)]: basicStartReducer,

    [combineActions(actions.getSummaries.fail)]: basicFailReducer,

    [actions.getSummaries.complete]: (state, action) => {
      const { summaries, } = action.payload

      return {
        ...basicLoadCompleteReducer(state, action),
        summaries,
      }
    },
  },
  initialState
)
