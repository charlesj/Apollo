import React from 'react';
import HomeHealth from './health/HomeHealth';
import Summary from './Summary';
import Weather from './weather/Weather';
import ObliqueStrategies from './thinking/ObliqueStrategies';

class Home extends React.Component {
  render() {
    return (
      <div>
        <Weather />
        <Summary />
        <HomeHealth />
        <ObliqueStrategies />
      </div>
    )
  }
}

module.exports = Home;
