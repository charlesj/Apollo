import React from 'react'
import PropTypes from 'prop-types'
import '../../styles/weather-icons.css'
import '../../styles/weather-icons-wind.css'

class WeatherIcon extends React.Component {
  getClassNames() {
    return 'wi ' + this.props.icon
  }

  render() {
    return <i className={() => this.getClassNames()} />
  }
}

WeatherIcon.propTypes = {
  icon: PropTypes.string.isRequired,
}

export default WeatherIcon
