import React from 'react'
import PropTypes from 'prop-types'
import '../../styles/weather-icons.css'
import '../../styles/weather-icons-wind.css'
import ClassNames from 'classnames'

function WeatherIcon(props) {
  return <i className={ClassNames('wi', props.icon)} />
}

WeatherIcon.propTypes = {
  icon: PropTypes.string.isRequired,
}

export default WeatherIcon
