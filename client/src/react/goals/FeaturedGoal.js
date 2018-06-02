import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, } from 'recharts'
import countdown from 'countdown'
import PropTypes from 'prop-types'

import { Container, FlexRow, Card, } from '../_controls'
import { goalActions, } from '../../redux/actions'
import { goalSelectors, } from '../../redux/selectors'

import './FeaturedGoal.css'

const countdownUnits =
  countdown.YEARS |
  countdown.MONTHS |
  countdown.DAYS |
  countdown.HOURS |
  countdown.MINUTES

const getDisplay = date => {
  return countdown(date, null, countdownUnits).toString()
}

class FeaturedGoal extends Component {
  componentDidMount() {
    this.props.loadGoals()
  }

  render() {
    const { goal, } = this.props
    if (!goal) {
      return <div />
    }

    return (
      <Container className="featuredGoal">
        <FlexRow>
          <div className="featuredGoalInfo">
            <h1>{goal.title}</h1>
            <div className="goalDescription">{goal.description}</div>
            <FlexRow>
              <Card title="Remaining" content={goal.actualDiff} />
              <Card title="Target" content={goal.targetValue} />
              <Card title="Completed" content={goal.completedDiff} />
            </FlexRow>

            <div className="progressBar">
              {goal.percentComplete > 0 && (
                <div
                  className="progressBarProgress"
                  style={{ width: goal.percentComplete + '%', }}
                >
                  {goal.percentComplete}%
                </div>
              )}
              {goal.percentComplete < 0 && (
                <div
                  className="progressBarNegativeProgress"
                  style={{ width: goal.percentComplete * -1 + '%', }}
                >
                  {goal.percentComplete}%
                </div>
              )}
            </div>
            <div className="goalCountDown">
              <strong>{getDisplay(new Date(goal.endDate))}</strong>{' '}
              left to make this happen. Get to work, you lazy bum.
            </div>
          </div>
          <div className="chartContainer">
            <LineChart width={600} height={300} data={goal.graphData}>
              <XAxis dataKey="key" />
              <YAxis domain={[goal.graphMin, goal.graphMax,]} />
              <CartesianGrid strokeDasharray="3 3" />
              <Line type="monotone" dataKey="val" stroke="#0A6640" />
            </LineChart>
          </div>
        </FlexRow>
      </Container>
    )
  }
}

FeaturedGoal.propTypes = {
  goal: PropTypes.object,
  loadGoals: PropTypes.func.isRequired,
}

FeaturedGoal.defaultProps = {
  goal: null,
}

function mapStateToProps(state) {
  const goal = goalSelectors.selectFeaturedGoal(state)

  return {
    goal,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    loadGoals: () => dispatch(goalActions.getGoals()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(FeaturedGoal)
