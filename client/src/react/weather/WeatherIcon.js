import React from "react";

import "../../styles/weather-icons.css";
import "../../styles/weather-icons-wind.css";

class WeatherIcon extends React.Component {
  constructor(props) {
    super(props);

    this.getClassNames = this.getClassNames.bind(this);
  }

  getClassNames() {
    return "wi " + this.props.icon;
  }

  render() {
    return <i className={this.getClassNames()} />;
  }
}

export default WeatherIcon;
