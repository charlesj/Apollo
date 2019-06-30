import React from 'react'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'
import './FlexRow.css'

function FlexRow(props) {
  return (
    <div
      className={ClassNames({
        flexRow: true,
        'flexRow-wrap': props.wrap,
        [props.className]: props.className,
      })}
    >
      {props.children}
    </div>
  )
}

FlexRow.propTypes = {
  className: PropTypes.string,
  children: PropTypes.array,
  wrap: PropTypes.bool,
}

export default FlexRow
