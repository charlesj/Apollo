import React from 'react'
import PropTypes from 'prop-types'
import { connect, } from 'react-redux'
import Flexbox from 'flexbox-react'
import {
  AddButton,
} from '../_controls'
import { financialSelectors, } from '../../redux/selectors'
import { financialActions, } from '../../redux/actions'

import TransactionLineItem from './TransactionLineItem'
import TransactionForm from './TransactionForm'
import './accountDisplay.css'

function AccountDisplay (props){
  const {
    account,
    transactions,
    selectTransaction,
    selectedTransaction,
    saveTransaction,
  } = props

  return (<Flexbox flexDirection='column'>
    <div className='accountDescription'>
      {account.description}
    </div>
    <Flexbox flexDirection='column' className='lineItems'>
      { transactions.map(t => <TransactionLineItem key={`transaction-${t.id}`} transaction={t} selectTransaction={selectTransaction} />)}
    </Flexbox>
    <AddButton noun='Transaction' onClick={() => selectTransaction({id: 'new',})} />
    { selectedTransaction && <TransactionForm
      transaction={selectedTransaction}
      onCancel={() => selectTransaction(null)}
      onSubmit={transaction => saveTransaction(transaction)}
    />}
  </Flexbox>)
}



AccountDisplay.propTypes = {
  account: PropTypes.object.isRequired,
  transactions: PropTypes.array.isRequired,
  loadTransactions: PropTypes.func.isRequired,
  saveTransaction: PropTypes.func.isRequired,
  selectTransaction: PropTypes.func.isRequired,
  selectedTransaction: PropTypes.object,
}

function mapStateToProps(state, props){
  return {
    transactions: financialSelectors.transactions(state, props.account.id),
    selectedTransaction: financialSelectors.selectedTransaction(state, props.account.id),
  }
}

function mapDispatchToProps(dispatch, { account, }){
  return {
    loadTransactions: () => dispatch(financialActions.loadTransactions(account.id)),
    saveTransaction: (transaction) => dispatch(financialActions.saveTransaction(account.id, transaction)),
    selectTransaction: (transaction) => dispatch(financialActions.actions.selectTransaction(transaction, account.id)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(AccountDisplay)