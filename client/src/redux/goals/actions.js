import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'
import { NotifySuccess, } from '../../services/notifier'
import { loadMetrics, } from '../metrics/actions'

const actionCreators = createActions({
  goals: {
    getGoals: basicActions(),
    upsertGoal: basicActions(),
  },
})

const actions = actionCreators.goals

export default actions

export function getGoals() {
  return dispatchBasicActions(actions.getGoals, async (dispatch) => {
    const goals = await apolloServer.invoke('getGoals', {})

    if (Array.isArray(goals)) {
      for(const goal of goals){
        dispatch(loadMetrics(null, goal.metricName))
      }
      return { goals, }
    } else {
      return { goals: [], }
    }
  })
}

export function upsertGoal(goal) {
  return dispatchBasicActions(actions.upsertGoal, async () => {
    await apolloServer.invoke('upsertGoal', { goal, })
    NotifySuccess('Saved Goal')
    return { goal, }
  })
}
