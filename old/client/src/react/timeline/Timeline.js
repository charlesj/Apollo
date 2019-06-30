import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { timelineSelectors, } from '../../redux/selectors'
import { timelineActions, } from '../../redux/actions'

import { Container, } from '../_controls'

class Timeline extends Component {
  componentDidMount() {
    this.props.load()
  }

  render() {
    const { entries, } = this.props

    return (
      <Container>
        Timeline
        {entries.map(entry => {
          return (
            <div key={entry.id}>
              {entry.eventTimeDisplay}: {entry.title}
            </div>
          )
        })}
      </Container>
    )
  }
}

Timeline.propTypes = {
  load: PropTypes.func.isRequired,
  entries: PropTypes.array.isRequired,
}

function mapStateToProps(state) {
  return {
    entries: timelineSelectors.all(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    load: () => dispatch(timelineActions.load()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Timeline)
