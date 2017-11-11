import React from 'react';


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

module.exports = WeatherIcon;
