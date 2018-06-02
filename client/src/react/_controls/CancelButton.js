import React from 'react'
import Button from './Button'
import FontAwesome from 'react-fontawesome'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'

function CancelButton(props) {
  const { className, } = props
  return (
    <Button
      {...props}
      type="button"
      className={ClassNames(className, 'cancelButton')}
    >
      <FontAwesome name="ban" /> Cancel
    </Button>
  )
}

CancelButton.propTypes = {
  className: PropTypes.string,
}

export default CancelButton
