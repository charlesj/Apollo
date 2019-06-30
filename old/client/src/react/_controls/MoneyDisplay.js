import React from 'react'
import Flexbox from 'flexbox-react'
import PropTypes from 'prop-types'
import ClassNames from 'classnames'

import './MoneyDisplay.css'

function MoneyDisplay({amount,}){
  const displayAmount = parseFloat(Math.round(amount * 100) / 100).toFixed(2)
  return (<Flexbox width='100px'>
    <Flexbox>$</Flexbox>
    <Flexbox flexGrow={1} justifyContent='flex-end' className={ClassNames({
      'negativeAmount': amount < 0,
    })}>{displayAmount}</Flexbox>
  </Flexbox>)
}

MoneyDisplay.propTypes ={
  amount: PropTypes.number.isRequired,
}

export default MoneyDisplay