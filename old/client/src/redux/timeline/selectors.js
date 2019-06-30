import moment from 'moment'
import _ from 'lodash'
import { keyedIdToArray, } from '../selector-helpers'

export function all(state) {
  const all = keyedIdToArray(state.timeline.entries).map(entry => {
    return {
      ...entry,
      eventTimeDisplay: moment(entry.event_time).calendar(),
    }
  })

  return _.orderBy(all, 'event_time', 'desc')
}
