import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'

const actionCreators = createActions({
  checklists: {
    getChecklists: basicActions(),
    selectChecklist: checklist => checklist,
    saveChecklist: basicActions(),
    removeChecklist: basicActions(),
    addCompletedChecklist: basicActions(),
    getCompletedChecklists: basicActions(),
    getChecklistCompletionLog: basicActions(),
  },
})

const actions = actionCreators.checklists

export default actions

export function getChecklists() {
  return dispatchBasicActions(actions.getChecklists, async () => {
    const checklists = await apolloServer.invoke('getChecklists', {})

    if (Array.isArray(checklists)) {
      return { checklists, }
    } else {
      return { checklists: [], }
    }
  })
}

export function saveChecklist(checklist) {
  return dispatchBasicActions(actions.saveChecklist, async () => {
    const updated = await apolloServer.invoke('saveChecklist', { checklist, })
    return { checklist: updated, }
  })
}

export function removeChecklist(checklist) {
  return dispatchBasicActions(actions.removeChecklist, async () => {
    await apolloServer.invoke('removeChecklist', { id: checklist.id, })
    return { checklist, }
  })
}

export function getCompletedChecklists(checklist_id) {
  return dispatchBasicActions(actions.getCompletedChecklists, async () => {
    const completedChecklists = await apolloServer.invoke(
      'getCompletedChecklists',
      { id: checklist_id, }
    )

    if (Array.isArray(completedChecklists)) {
      return { completedChecklists, }
    } else {
      return { completedChecklists: [], }
    }
  })
}

export function addCompletedChecklist(checklist_id, notes, items) {
  return dispatchBasicActions(actions.addCompletedChecklist, async () => {
    const completedChecklist = await apolloServer.invoke(
      'addCompletedChecklist',
      { checklist_id, notes, items, }
    )
    return { completedChecklist, }
  })
}

export function getCompletedChecklist() {
  return dispatchBasicActions(actions.getCompletedChecklist, async () => {
    const checklist = await apolloServer.invoke('getCompletedChecklist', {})

    return { checklist, }
  })
}

export function getChecklistCompletionLog() {
  return dispatchBasicActions(actions.getChecklistCompletionLog, async () => {
    const checklistLog = await apolloServer.invoke(
      'getChecklistCompletionLog',
      {}
    )

    if (Array.isArray(checklistLog)) {
      return { checklistLog, }
    } else {
      return { checklistLog: [], }
    }
  })
}
