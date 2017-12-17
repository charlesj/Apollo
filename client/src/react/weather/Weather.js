import React from "react";
import moment from "moment";
import apolloServer from "../../services/apolloServer";
import WeatherIcon from "./WeatherIcon";
import FontAwesome from "react-fontawesome";

import "./Weather.css";

var DarkSkyIconMapping = {
  "clear-day": "wi-day-sunny",
  "clear-night": "wi-night-clear",
  rain: "wi-day-rain",
  snow: "wi-day-snow",
  sleet: "wi-day-rain-mix",
  wind: "wi-day-cloudy-gusts",
  fog: "wi-day-fog",
  cloudy: "wi-day-cloudy",
  "partly-cloudy-day": "wi-day-cloudy-high",
  "partly-cloudy-night": "wi-night-cloudy-high",
  hail: "wi-day-hail",
  thunderstorm: "wi-day-lightning",
  tornado: "wi-tornado"
};

function PrecipDisplay(props) {
  var percentDisplay = Math.round(props.chance * 100);
  if (percentDisplay === 0) {
    return <div>No Precipitation</div>;
  }
  return (
    <div>
      {percentDisplay}% chance for {props.precipType}
    </div>
  );
}

function ForecastDay(props) {
  var date = moment.unix(props.f.time);
  return (
    <div className="forecastDay">
      <div>
        {date.format("dddd")} {date.format("MMM Do")}
      </div>
      <div className="weatherIcon">
        <WeatherIcon icon={DarkSkyIconMapping[props.f.icon]} />
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
  );
}

function ForecastDisplay(props) {
  if (!props.data) {
    return (
      <div className="weatherForecast">There are no forecasts configured</div>
    );
  }
  var curr = props.data.Forecast.currently;
  return (
    <div className="weatherForecast">
      <div className="weatherLocation">
        {props.data.Location}
        <div className="weeklySummary">
          {" "}
          - {props.data.Forecast.daily.summary}
        </div>
      </div>
      <div className="forecastsContainer">
        <div className="forecastDay">
          Now
          <div className="weatherIcon">
            <WeatherIcon icon={DarkSkyIconMapping[curr.icon]} />
          </div>
          <div>
            {curr.temperature}℉ (feels: {curr.apparentTemperature} ℉){" "}
          </div>
          <div className="forecastSummary">{curr.summary} </div>
          <PrecipDisplay
            chance={curr.precipProbability}
            precipType={curr.precipType}
          />
          <div>{Math.round(curr.humidity * 100)}% humidity</div>
        </div>

        {props.data.Forecast.daily.data.map((d, i) => {
          return <ForecastDay key={i} f={d} />;
        })}
      </div>
      <div>
        {props.data.Forecast.alerts &&
          props.data.Forecast.alerts.map((a, i) => {
            var expiresOn = moment.unix(a.expires);
            return (
              <div className="weatherAlerts" key={i}>
                <FontAwesome name="exclamation-triangle" />{" "}
                <a href={a.uri} target="_blank">
                  {a.title}
                </a>
                - expires {expiresOn.calendar()}
                <div className="alertDescription">{a.description}</div>
              </div>
            );
          })}
      </div>
    </div>
  );
}

class Weather extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      data: null
    };
  }

  componentDidMount() {
    apolloServer.invoke("getWeatherForecasts", {}).then(data => {
      this.setState({
        data
      });
    });
  }

  render() {
    if (this.state.data == null || this.state.data.forecasts == null) {
      return <div className="weatherContainer">Loading Weather...</div>;
    }

    return (
      <div className="weatherContainer">
        {this.state.data.forecasts.map((f, i) => {
          return <ForecastDisplay key={i} data={f} />;
        })}
      </div>
    );
  }
}

export default Weather;
