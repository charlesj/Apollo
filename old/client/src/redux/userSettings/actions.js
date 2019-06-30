import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'
import { NotifySuccess, } from '../../services/notifier'
const actionCreators = createActions({
  userSettings: {
    load: basicActions(),
    save: basicActions(),
    select: setting => setting,
  },
})

export const actions = actionCreators.userSettings

export function load() {
  return dispatchBasicActions(actions.load, async () => {
    const settings = await apolloServer.invoke('getAllUserSettings', {})
    return settings
  })
}

export function save(name, value) {
  return dispatchBasicActions(actions.save, async () => {
    const updated = await apolloServer.invoke('saveUserSetting', {
      name,
      value,
    })
    NotifySuccess(`Updated ${name}`)
    return { setting: updated, }
  })
}
