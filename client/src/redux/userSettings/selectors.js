import moment from 'moment'
import _ from 'lodash'
import { keyedIdToArray, } from '../selector-helpers'

export function all(state) {
  const all = keyedIdToArray(state.userSettings.settings).map(setting => {
    return {
      ...setting,
      createdAtDisplay: moment(setting.created_at).calendar(),
      updatedAtDisplay: moment(setting.updated_at).calendar(),
    }
  })

  return _.orderBy(all, 'name')
}

export function selected(state) {
  return state.userSettings.selectedSetting
}
