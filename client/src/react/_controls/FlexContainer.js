import React from 'react'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'

function FlexContainer(props) {
  return (
    <div
      className={ClassNames({
        [props.className]: props.className,
        'flexContainer-grow': props.grow,
      })}
    >
      {props.children}
    </div>
  )
}

FlexContainer.propTypes = {
  className: PropTypes.string,
  children: PropTypes.any,
  grow: PropTypes.bool,
}

export default FlexContainer
