import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'

const actionCreators = createActions({
  timeline: {
    load: basicActions(),
  },
})

export const actions = actionCreators.timeline

export function load(start, end) {
  return dispatchBasicActions(actions.load, async () => {
    const entries = await apolloServer.invoke('gettimeline', { start, end, })
    return entries
  })
}
