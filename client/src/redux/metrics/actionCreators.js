import { createActions, } from 'redux-actions'
import { basicActions, } from '../redux-helpers'

const actionCreators = createActions({
  metrics: {
    loadMetrics: basicActions(),
    addMetrics: basicActions(),
  },
})

const actions = actionCreators.metrics

export default actions
