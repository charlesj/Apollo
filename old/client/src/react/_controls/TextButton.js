import React from 'react'
import ClassNames from 'classnames'
import Button from './Button'
import PropTypes from 'prop-types'

function TextButton(props) {
  const { className, ...rest } = props
  return (
    <Button className={ClassNames('textButton', className)} {...rest}>
      {props.children}
    </Button>
  )
}

TextButton.propTypes = {
  className: PropTypes.string,
  children: PropTypes.any,
}

export default TextButton
