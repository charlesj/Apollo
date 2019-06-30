import React from 'react'
import PropTypes from 'prop-types'
import Flexbox from 'flexbox-react'
import {
  MoneyDisplay,
  TextButton,
} from '../_controls'

function TransactionLineItem({transaction, selectTransaction,}){
  return <Flexbox flexDirection='row'>
    <Flexbox width='50px'>{transaction.id.toString().padStart(5, '0')}</Flexbox>
    <Flexbox flexGrow={1}><TextButton onClick={() => selectTransaction(transaction)}>{transaction.name}</TextButton></Flexbox>
    <Flexbox><MoneyDisplay amount={transaction.amount} /></Flexbox>
  </Flexbox>
}

TransactionLineItem.propTypes = {
  transaction: PropTypes.object.isRequired,
  selectTransaction: PropTypes.func.isRequired,
}

export default TransactionLineItem