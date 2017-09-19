import React from 'react';
import HomeHealth from './health/HomeHealth';
import Summary from './Summary';
import Weather from './weather/Weather';

class Home extends React.Component {
  render() {
    return (
      <div>
        <Summary />
        <Weather />
        <hr />
        <HomeHealth />
      </div>
    )
  }
}

module.exports = Home;
