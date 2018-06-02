import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { summaryActions, } from '../../redux/actions'
import { Card, FlexRow, } from '../_controls'

class Summaries extends Component {
  componentDidMount() {
    this.props.getSummaries()
  }

  render() {
    const { summaries, } = this.props
    return (
      <FlexRow wrap>
        {summaries.map(summary => {
          return (
            <Card
              title={summary.label}
              content={summary.amount}
              key={summary.id}
            />
          )
        })}
      </FlexRow>
    )
  }
}

Summaries.propTypes = {
  summaries: PropTypes.array.isRequired,
  getSummaries: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  const { summaries, } = state.summaries
  return {
    summaries,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    getSummaries: () => dispatch(summaryActions.getSummaries()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Summaries)
