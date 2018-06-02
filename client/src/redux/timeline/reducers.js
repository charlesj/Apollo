import { combineActions, handleActions, } from 'redux-actions'
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer,
} from '../redux-helpers'

import { actions, } from './actions'

const initialState = {
  entries: {},
}

export default handleActions(
  {
    [combineActions(actions.load.start)]: basicStartReducer,

    [combineActions(actions.load.fail)]: basicFailReducer,

    [actions.load.complete]: (state, action) => {
      const entries = action.payload

      return {
        ...basicLoadCompleteReducer(state, action),
        entries: idReducer(state.entries, entries),
      }
    },
  },
  initialState
)
