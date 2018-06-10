import { keyedIdToArray, } from '../selector-helpers'


function accounts(state){
  return keyedIdToArray(state.financial.accounts)
}

function selectedAccount(state){
  if(state.financial.selectedAccount === 'new'){
    return {}
  }
  return state.financial.accounts[state.financial.selectedAccount]
}

function accountMode(state){
  return state.financial.accountMode
}

function transactions(state, accountId){
  return keyedIdToArray(state.financial.transactions[accountId] || {})
}

function selectedTransaction(state, accountId){
  if(state.financial.selectedTransaction[accountId] == null){
    return null
  }

  if(state.financial.selectedTransaction[accountId] === 'new'){
    return {}
  }

  return state.financial.transactions[accountId][state.financial.selectedTransaction[accountId]]
}

export {
  accountMode,
  accounts,
  selectedAccount,
  transactions,
  selectedTransaction,
}