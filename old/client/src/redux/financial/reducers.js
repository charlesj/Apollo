import { combineActions, handleActions, } from 'redux-actions'
import {
  basicFailReducer,
  basicLoadCompleteReducer,
  basicStartReducer,
  idReducer,
} from '../redux-helpers'
import { actions, } from './actions'

const initialState = {
  accountMode: 'view',
  accounts: [],
  selectedAccount: null,
  selectedTransaction: {},
  transactions: {},
}

const transformTransaction = (t) => ({
  ...t,
  tagsDisplay:t.tags.join(', '),
  occurred_at: t.occurred_at.split('+')[0],
})


export default handleActions(
  {
    [combineActions(
      actions.loadAccounts.start,
    )]: basicStartReducer,

    [combineActions(
      actions.loadAccounts.fail,
    )]: basicFailReducer,

    [actions.loadAccounts.complete]: (state, action) => {
      const { accounts, } = action.payload
      const selectedAccount = state.selectedAccount || (accounts.length > 0 && accounts[0].id)
      return {
        ...basicLoadCompleteReducer(state, action),
        accounts: idReducer(state.accounts, accounts),
        selectedAccount,
      }
    },

    [actions.loadTransactions.complete]: (state, action) => {
      const { payload: { transactions, accountId, },} = action
      return {
        ...basicLoadCompleteReducer(state, action),
        transactions: {
          ...state.transactions,
          [accountId]: idReducer(state.transactions[accountId] || {}, transactions.map(transformTransaction)),
        },
      }
    },

    [actions.saveAccount.complete]: (state, action) => {
      const { account, } = action.payload
      return {
        ...basicLoadCompleteReducer(state, action),
        accounts: idReducer(state.accounts, account),
        accountMode: 'view',
        selectedAccount: account.id,
      }
    },

    [actions.saveTransaction.complete]: (state, action) => {
      const { payload: { transaction, accountId, },} = action

      return {
        ...basicLoadCompleteReducer(state, action),
        transactions: {
          ...state.transactions,
          [accountId]: idReducer(state.transactions[accountId] || {}, transformTransaction(transaction)),
        },
        selectedTransaction:{
          ...state.selectedTransaction,
          [accountId]: null,
        },
      }
    },

    [actions.setAccountMode]: (state, action) => {
      return {
        ...state,
        accountMode: action.payload,
      }
    },

    [actions.selectAccount]: (state, action) => {
      return {
        ...state,
        selectedAccount: action.payload.id,
        accountMode: action.payload.id === 'new' ? 'edit' : 'view',
      }
    },

    [actions.selectTransaction]: (state, action) => {
      return {
        ...state,
        selectedTransaction: {
          ...state.selectedTransaction,
          [action.payload.accountId]: action.payload.transaction ? action.payload.transaction.id : null,
        },
      }
    },
  },
  initialState
)
