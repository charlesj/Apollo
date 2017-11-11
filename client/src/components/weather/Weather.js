import React from 'react';
import moment from 'moment';
import apolloServer from '../../services/apollo-server';
import WeatherIcon from './WeatherIcon';

var DarkSkyIconMapping = {
  "clear-day": "wi-day-sunny",
  "clear-night": "wi-night-clear",
  "rain": "wi-day-rain",
  "snow": "wi-day-snow",
  "sleet": "wi-day-rain-mix",
  "wind": "wi-day-cloudy-gusts",
  "fog": "wi-day-fog",
  "cloudy": "wi-day-cloudy",
  "partly-cloudy-day": "wi-day-cloudy-high",
  "partly-cloudy-night": "wi-night-cloudy-high",
  "hail": "wi-day-hail",
  "thunderstorm": "wi-day-lightning",
  "tornado": "wi-tornado"
}

function ForecastDay(props) {
  console.log(props.f);
  var date = moment.unix(props.f.time);
  return (<div className='forecastDay'>
    <div><WeatherIcon icon={DarkSkyIconMapping[props.f.icon]} /></div>
    <div>{date.calendar()}</div>
    <div>{props.f.summary}</div>
    <div>H {props.f.temperatureHigh}℉</div>
    <div>L {props.f.temperatureLow}℉</div>
  </div>);
}

function ForecastDisplay(props) {
  return (<div className='weatherForecast'>
    <div className='currentConditions'>
    <div className='weatherIcon'>
    <WeatherIcon icon={DarkSkyIconMapping[props.data.Forecast.currently.icon]} />
    </div>
    {props.data.Location} - {props.data.Forecast.currently.summary} <br />
    {props.data.Forecast.currently.temperature}℉
    </div>
    {props.data.Forecast.daily.data.map((d, i) => {
      return <ForecastDay key={i} f={d} />
    })}
  </div>)
}

class Weather extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      data: null
    };
  }

  componentDidMount() {
    apolloServer.invoke('getWeatherForecasts', {})
      .then(data => {
        this.setState({
          data
        });
      });
  }

  render() {
    if (this.state.data === null) {
      return (<div className='weatherContainer'>Loading Weather...</div>);
    }

    return (<div className='weatherContainer'>
      { this.state.data.forecasts.map((f, i) => {
        return (<ForecastDisplay key={i} data={f} />)
      }) }
    </div>);
  }
}


module.exports = Weather;
