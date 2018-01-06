import React, { Component } from "react";
import { connect } from "react-redux";
import { LineChart, Line, XAxis, YAxis, CartesianGrid } from "recharts";
import countdown from "countdown";

import { Container, FlexRow, Card } from "../_controls";
import { metricsActions } from "../../redux/actions";
import { goalSelectors } from "../../redux/selectors";

import "./FeaturedGoal.css";

let loadedMetrics = "";

const countdownUnits =
  countdown.YEARS |
  countdown.MONTHS |
  countdown.DAYS |
  countdown.HOURS |
  countdown.MINUTES;

const getDisplay = date => {
  return countdown(date, null, countdownUnits).toString();
};

class FeaturedGoal extends Component {
  componentWillReceiveProps(props) {
    const { goal, loadMetrics } = props;
    if (goal.metricName !== loadedMetrics) {
      loadMetrics(goal.metricName);
      loadedMetrics = goal.metricName;
    }
  }

  render() {
    const { goal } = this.props;
    if (!goal) {
      return <div />;
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
              <div
                className="progressBarProgress"
                style={{ width: goal.percentComplete + "%" }}
              >
                {goal.percentComplete}%
              </div>
            </div>
            <div className="goalCountDown">
              You've got <strong>{getDisplay(new Date(goal.endDate))}</strong>{" "}
              to make this happen. Get to work, you lazy bum.
            </div>
          </div>
          <div className="chartContainer">
            <LineChart width={600} height={300} data={goal.graphData}>
              <XAxis dataKey="key" />
              <YAxis domain={[goal.graphMin, goal.graphMax]} />
              <CartesianGrid strokeDasharray="3 3" />
              <Line type="monotone" dataKey="val" stroke="#0A6640" />
            </LineChart>
          </div>
        </FlexRow>
      </Container>
    );
  }
}

function mapStateToProps(state, props) {
  const goal = goalSelectors.selectFeaturedGoal(state);

  if (goal === null) {
    return { goal: null };
  }

  return {
    goal
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    loadMetrics: name => dispatch(metricsActions.loadMetrics(null, name))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(FeaturedGoal);
