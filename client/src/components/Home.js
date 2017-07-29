import React from 'react';
import HomeHealth from './health/HomeHealth';
import Summary from './Summary';

class Home extends React.Component {
  render() {
    return (
      <div>
        <Summary />
        <hr />
        <HomeHealth />
      </div>
    )
  }
}

module.exports = Home;
