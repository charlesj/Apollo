import React from 'react';

import Summary from './Summary';
import Weather from './weather/Weather';
import ObliqueStrategies from './thinking/ObliqueStrategies';

class Home extends React.Component {
  render() {
    return (
      <div>
        <Weather />
        <Summary />
        <ObliqueStrategies />
      </div>
    )
  }
}

module.exports = Home;
