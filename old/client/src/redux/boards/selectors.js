import _ from 'lodash'
import { keyedIdToArray, } from '../selector-helpers'

export function all(state) {
  return _.orderBy(keyedIdToArray(state.boards.boards), 'list_order')
}

export function nextListOrder(state) {
  const max = _.max(keyedIdToArray(state.boards.boards), 'list_order')
  if (!max) {
    return 0
  }
  return max.list_order + 1
}

export function all_items(state, board_id) {
  return keyedIdToArray(state.boards.boardItems).filter(i => {
    return i.board_id === board_id
  })
}

export function complete_items(state, board_id) {
  const items = all_items(state, board_id)
  return items.filter(i => i.completed_at)
}

export function incomplete_items(state, board_id) {
  const items = all_items(state, board_id)
  return items.filter(i => !i.completed_at)
}
