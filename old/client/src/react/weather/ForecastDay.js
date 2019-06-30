import React from 'react'
import moment from 'moment'
import PropTypes from 'prop-types'
import DarkSkyIconMap from './DarkSkyIconMapping'
import PrecipDisplay from './PrecipDisplay'
import WeatherIcon from './WeatherIcon'

function ForecastDay(props) {
  var date = moment.unix(props.f.time)
  return (
    <div className="forecastDay">
      <div>
        {date.format('dddd')} {date.format('MMM Do')}
      </div>
      <div className="weatherIcon">
        <WeatherIcon icon={DarkSkyIconMap[props.f.icon]} />
      </div>
      <div>
        L {props.f.temperatureLow}℉ - H {props.f.temperatureHigh}℉
      </div>
      <div className="forecastSummary">{props.f.summary}</div>
      <PrecipDisplay
        chance={props.f.precipProbability}
        precipType={props.f.precipType}
      />
      <div>{Math.round(props.f.humidity * 100)}% humidity</div>
    </div>
  )
}

ForecastDay.propTypes = {
  f: PropTypes.object.isRequired,
}

export default ForecastDay
