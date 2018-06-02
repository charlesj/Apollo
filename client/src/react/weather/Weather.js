import React from 'react'
import apolloServer from '../../services/apolloServer'
import { Container, } from '../_controls'
import Forecast from './Forecast'
import './Weather.css'

class Weather extends React.Component {
  constructor(props) {
    super(props)

    this.state = {
      data: null,
    }
  }

  componentDidMount() {
    apolloServer.invoke('getWeatherForecasts', {}).then(data => {
      this.setState({
        data,
      })
    })
  }

  render() {
    if (this.state.data == null || this.state.data.forecasts == null) {
      return <Container>Loading Weather...</Container>
    }

    return (
      <Container>
        {this.state.data.forecasts.map((f, i) => {
          return <Forecast key={i} data={f} />
        })}
      </Container>
    )
  }
}

export default Weather
