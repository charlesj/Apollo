import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'
import { NotifySuccess, } from '../../services/notifier'

const actionCreators = createActions({
  financial: {
    loadAccounts: basicActions(),
    loadTransactions: basicActions(),
    saveAccount: basicActions(),
    saveTransaction: basicActions(),
    selectAccount: account => account,
    selectTransaction: (transaction, accountId) => ({transaction, accountId,}),
    setAccountMode: mode => mode,
  },
})

export const actions = actionCreators.financial

export function loadAccounts() {
  return dispatchBasicActions(actions.loadAccounts, async (dispatch) => {
    const accounts = await apolloServer.invoke('getFinancialAccounts', {})
    accounts.map(a => dispatch(loadTransactions(a.id)))
    return { accounts, }
  })
}

export function loadTransactions(accountId){
  return dispatchBasicActions(actions.loadTransactions, async () => {
    const transactions = await apolloServer.invoke('getFinancialTransactions', {account_id: accountId,})
    return { transactions, accountId, }
  })
}

export function saveAccount(account){
  return dispatchBasicActions(actions.saveAccount, async () => {
    const saved = await apolloServer.invoke('upsertFinancialAccount', { account, })
    NotifySuccess('Saved Changes to Account', 'Financial')
    return { account: saved, }
  })
}

export function saveTransaction(accountId, transaction){
  return dispatchBasicActions(actions.saveTransaction, async () => {
    transaction.account_id = accountId
    transaction.tags = transaction.tagsDisplay.split(',')
    const saved = await apolloServer.invoke('upsertFinancialTransaction', {transaction,})
    NotifySuccess('Saved Transaction to Account', 'Financial')
    return { transaction: saved, accountId, }
  })
}

export function selectAccount(account){
  return dispatch => {
    dispatch(actions.selectAccount(account))
    dispatch(loadTransactions(account.id))
  }
}