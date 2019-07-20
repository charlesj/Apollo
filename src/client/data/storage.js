import { fromJS } from 'immutable'

const dataKey = 'apollo'

export const load = () => {
  const json = window.localStorage.getItem(dataKey)
  const state = json ? JSON.parse(json) : {}
  return fromJS(state)
}

export const save = (state) => {
  window.localStorage.setItem(dataKey, state.toJSON())
}
