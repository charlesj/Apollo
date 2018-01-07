import React, { Component } from "react";
import countdown from "countdown";
import FontAwesome from "react-fontawesome";

import { Container, TextButton } from "../_controls";
import config from "../../config";

import "./LifeCounter.css";

const { birthDate, endDate } = config;

const countdownUnits =
  countdown.YEARS |
  countdown.MONTHS |
  countdown.DAYS |
  countdown.HOURS |
  countdown.MINUTES;

const getDisplay = date => {
  return countdown(date, null, countdownUnits).toString();
};

const getSecondDisplay = date => {
  return countdown(date, null, countdown.SECONDS).toString();
};

class LifeCounter extends Component {
  constructor(props) {
    super(props);
    this.state = {
      pastDisplay: getDisplay(birthDate),
      futureDisplay: getDisplay(endDate),
      secondDisplay: getSecondDisplay(endDate),
      secondMode: true
    };

    this.updateDisplay = this.updateDisplay.bind(this);
  }

  componentDidMount() {
    var intervalId = setInterval(this.updateDisplay, 1000);
    this.setState({ intervalId: intervalId });
  }

  componentWillUnmount() {
    clearInterval(this.state.intervalId);
  }

  updateDisplay() {
    this.setState({
      pastDisplay: getDisplay(birthDate),
      futureDisplay: getDisplay(endDate),
      secondDisplay: getSecondDisplay(endDate)
    });
  }

  render() {
    const {
      pastDisplay,
      futureDisplay,
      secondDisplay,
      secondMode
    } = this.state;
    return (
      <Container className="lifeCounterContainer">
        <TextButton onClick={() => this.setState({ secondMode: !secondMode })}>
          <FontAwesome name="repeat" />
        </TextButton>
        {!secondMode && (
          <div>
            <div>Current Age: {pastDisplay}</div>
            <div>Estimate Remaining: {futureDisplay}</div>
          </div>
        )}
        {secondMode && <div> {secondDisplay}</div>}
      </Container>
    );
  }
}

export default LifeCounter;
