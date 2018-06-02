import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import { directions, } from '../enums'
import apolloServer from '../../services/apolloServer'

const actionCreators = createActions({
  boards: {
    load: basicActions(),
    saveBoard: basicActions(),
    moveBoard: basicActions(),
    removeBoard: basicActions(),
    loadItems: basicActions(),
    saveItem: basicActions(),
    removeItem: basicActions(),
  },
})

const actions = actionCreators.boards

export default actions

export function load() {
  return dispatchBasicActions(actions.load, async () => {
    const boards = await apolloServer.invoke('getBoards', {})
    return { boards, }
  })
}

export function saveBoard(board) {
  return dispatchBasicActions(actions.saveBoard, async () => {
    const updated = await apolloServer.invoke('saveBoard', { board, })
    return updated
  })
}

export function moveBoard({ direction, boards, index, }) {
  return dispatchBasicActions(actions.moveBoard, async () => {
    let primaryBoard = boards[index]
    let secondaryBoard
    const length = boards.length
    if (direction === directions.left && index > 0) {
      secondaryBoard = boards[index - 1]
    }

    if (direction === directions.right && index < length) {
      secondaryBoard = boards[index + 1]
    }

    if (primaryBoard.list_order === secondaryBoard.list_order) {
      secondaryBoard.list_order = primaryBoard.list_order + 1
    }

    const swap = primaryBoard.list_order
    primaryBoard.list_order = secondaryBoard.list_order
    secondaryBoard.list_order = swap

    const updated = []
    updated.push(
      await apolloServer.invoke('saveBoard', { board: primaryBoard, })
    )
    if (secondaryBoard) {
      updated.push(
        await apolloServer.invoke('saveBoard', { board: secondaryBoard, })
      )
    }
    return { boards: updated, }
  })
}

export function removeBoard(board) {
  return dispatchBasicActions(actions.removeBoard, async () => {
    await apolloServer.invoke('deleteBoard', {
      id: board.id,
    })

    return { board, }
  })
}

export function loadItems(board_id) {
  return dispatchBasicActions(actions.loadItems, async () => {
    const items = await apolloServer.invoke('getBoardItems', {
      board_id,
    })

    return { items, }
  })
}

export function saveItem(boardItem) {
  return dispatchBasicActions(actions.saveItem, async () => {
    const item = await apolloServer.invoke('saveBoardItem', boardItem)
    return { item, }
  })
}

export function removeItem(boardItem) {
  return dispatchBasicActions(actions.removeItem, async () => {
    await apolloServer.invoke('DeleteBoardItem', boardItem)
    return { id: boardItem.id, }
  })
}
