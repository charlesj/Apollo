import React from 'react'
import Button from './Button'
import FontAwesome from 'react-fontawesome'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'

function LoadMoreButton(props) {
  const { className, } = props
  return (
    <Button
      {...props}
      primary
      className={ClassNames(className, 'loadMoreButton')}
    >
      <FontAwesome name="chevron-circle-right" /> Load More
    </Button>
  )
}

LoadMoreButton.propTypes = {
  className: PropTypes.string,
}

export default LoadMoreButton
