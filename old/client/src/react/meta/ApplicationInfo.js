import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import moment from 'moment'
import PropTypes from 'prop-types'
import { metaActions, } from '../../redux/actions'

class ApplicationInfo extends Component {
  constructor(props) {
    super(props)
    this.state = {
      compiledDisplay: null,
    }
  }

  componentDidMount() {
    this.props.load()
    var intervalId = setInterval(() => this.updateDisplay(), 1000)
    this.setState({ intervalId: intervalId, })
  }

  updateDisplay() {
    if (this.props.applicationInfo) {
      const { compiledOn, } = this.props.applicationInfo
      if (compiledOn) {
        this.setState({ compiledDisplay: moment(compiledOn).calendar(), })
      }
    }
  }

  componentWillUnmount() {
    clearInterval(this.state.intervalId)
  }

  render() {
    const { version, commitHash, } = this.props.applicationInfo
    const { compiledDisplay, } = this.state

    return (
      <div className="applicationInfo">
        Apollo <strong>{version}</strong> ({commitHash &&
          commitHash.slice(0, 6)}) - {compiledDisplay}
      </div>
    )
  }
}

ApplicationInfo.propTypes = {
  applicationInfo: PropTypes.object,
  load: PropTypes.func.isRequired,
}

ApplicationInfo.defaultProps = {
  applicationInfo: null,
}

function mapStateToProps(state) {
  const { applicationInfo, } = state.meta

  return {
    applicationInfo,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    load: () => dispatch(metaActions.applicationInfo()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(ApplicationInfo)
