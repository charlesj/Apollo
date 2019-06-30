import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import moment from 'moment'
import apolloServer from '../../services/apolloServer'
import loginService from '../../services/loginService'

const actionCreators = createActions({
  meta: {
    login: basicActions(),
    logout: basicActions(),
    notify: basicActions(),
    incrementRequests: () => 1,
    decrementRequests: () => 1,
    applicationInfo: basicActions(),
  },
})

const actions = actionCreators.meta

export default actions

export function notify({ type, message, }) {
  return dispatchBasicActions(actions.notify, () => {
    return { type, message, unread: true, time: moment(), }
  })
}

export function login(password) {
  return dispatchBasicActions(actions.login, async () => {
    const loginResult = await apolloServer.invoke('Login', {
      password,
    })

    if (loginResult.token) {
      return { token: loginResult.token, }
    } else {
      return { loginError: true, }
    }
  })
}

export function logout() {
  return dispatchBasicActions(actions.logout, async () => {
    var token = loginService.getToken()
    await apolloServer.invoke('revokeLoginSession', {
      tokenToRevoke: token,
    })
    return {
      result: true,
    }
  })
}

export function incrementRequests() {
  return dispatch => {
    dispatch(actions.incrementRequests())
  }
}

export function decrementRequests() {
  return dispatch => {
    dispatch(actions.decrementRequests())
  }
}

export function applicationInfo() {
  return dispatchBasicActions(actions.applicationInfo, async () => {
    const applicationInfo = await apolloServer.invoke('applicationInfo', {})
    return { applicationInfo, }
  })
}
