import React, { Component } from "react";
import countdown from "countdown";
import "./LifeCounter.css";
import config from "../../config";

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

class LifeCounter extends Component {
  constructor(props) {
    super(props);
    this.state = {
      pastDisplay: getDisplay(birthDate),
      futureDisplay: getDisplay(endDate)
    };

    this.updateDisplay = this.updateDisplay.bind(this);
    setInterval(this.updateDisplay, 1000);
  }

  updateDisplay() {
    this.setState({
      pastDisplay: getDisplay(birthDate),
      futureDisplay: getDisplay(endDate)
    });
  }

  render() {
    return (
      <div className="lifeCounterContainer">
        <div>E: {this.state.futureDisplay}</div>
        <div>A: {this.state.pastDisplay}</div>
      </div>
    );
  }
}

export default LifeCounter;
