import React from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { metaSelectors, } from '../../redux/selectors'
import FontAwesome from 'react-fontawesome'

function ServerActivity(props) {
  if (props.isRequesting) {
    return (
      <div>
        <FontAwesome name="cog" spin />
      </div>
    )
  }
  return (
    <div>
      <FontAwesome name="cog" />
    </div>
  )
}

ServerActivity.propTypes = {
  isRequesting: PropTypes.bool.isRequired,
}

function mapStateToProps(state) {
  return {
    isRequesting: metaSelectors.activeServerRequests(state),
  }
}

export default connect(mapStateToProps)(ServerActivity)
