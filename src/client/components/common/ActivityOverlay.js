import React from 'react'
import PropTypes from 'prop-types'
import { CircularProgress } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import ClassNames from 'classnames'
import { Flex } from 'mui-blox'

const ActivityOverlay = ({
  className,
  isActive,
  children,
  isSmall,
  progressProps,
}) => {
  const classes = useStyles()

  return (
    <Flex
      flexDirection='column'
      flex='1'
      className={ClassNames(classes.root, className)}
    >
      {children}
      {isActive && (
        <Flex
          flex='1'
          justifyContent='center'
          alignItems='center'
          className={classes.overlay}
        />)}
      {isActive && (
        <Flex
          flex='1'
          justifyContent='center'
          alignItems='center'
          className={classes.waitingContent}
        >
          <CircularProgress
            size={isSmall ? 25 : 50}
            thickness={4}
            {...progressProps}
          />
        </Flex>
      )}
    </Flex>
  )
}

ActivityOverlay.propTypes = {
  isActive: PropTypes.bool.isRequired,
  isSmall: PropTypes.bool,
  progressProps: PropTypes.object,
}

ActivityOverlay.defaultProps = {
  isSmall: false,
  progressProps: {},
}

const useStyles = makeStyles(() => ({
  root: {
    position: 'relative',
  },
  waitingContent: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    zIndex: 10,
  },
  overlay: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    zIndex: 5,
    backgroundColor: 'white',
    opacity: .7,
  }
}))

export default ActivityOverlay
