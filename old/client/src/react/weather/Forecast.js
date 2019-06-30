import React from 'react'
import moment from 'moment'
import PropTypes from 'prop-types'
import FontAwesome from 'react-fontawesome'
import WeatherIcon from './WeatherIcon'
import DarkSkyIconMap from './DarkSkyIconMapping'
import PrecipDisplay from './PrecipDisplay'
import ForecastDay from './ForecastDay'

function Forecast(props) {
  if (!props.data) {
    return <div>There are no forecasts to display</div>
  }
  var curr = props.data.Forecast.currently
  return (
    <div>
      <div className="weatherLocation">
        {props.data.Location}
        <div className="weeklySummary">
          {' '}
          - {props.data.Forecast.daily.summary}
        </div>
      </div>
      <div className="forecastsContainer">
        <div className="forecastDay">
          Now
          <div className="weatherIcon">
            <WeatherIcon icon={DarkSkyIconMap[curr.icon]} />
          </div>
          <div>
            {curr.temperature}℉ (feels: {curr.apparentTemperature} ℉){' '}
          </div>
          <div className="forecastSummary">{curr.summary} </div>
          <PrecipDisplay
            chance={curr.precipProbability}
            precipType={curr.precipType}
          />
          <div>{Math.round(curr.humidity * 100)}% humidity</div>
        </div>

        {props.data.Forecast.daily.data.map((d, i) => {
          return <ForecastDay key={i} f={d} />
        })}
      </div>
      <div>
        {props.data.Forecast.alerts &&
          props.data.Forecast.alerts.map((a, i) => {
            var expiresOn = moment.unix(a.expires)
            return (
              <div className="weatherAlerts" key={i}>
                <FontAwesome name="exclamation-triangle" />
                <a href={a.uri} target="_blank">
                  {a.title}
                </a>
                - expires {expiresOn.calendar()}
                <div className="alertDescription">{a.description}</div>
              </div>
            )
          })}
      </div>
    </div>
  )
}

Forecast.propTypes = {
  data: PropTypes.object,
}

Forecast.defaultProps = {
  data: null,
}

export default Forecast
