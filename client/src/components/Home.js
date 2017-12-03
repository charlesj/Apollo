import React from 'react';

import Summary from './Summary';
import Weather from './weather/Weather';
import ObliqueStrategies from './thinking/ObliqueStrategies';
import CompleteChecklist from './checklists/CompleteChecklist';

class Home extends React.Component {
  render() {
    return (
      <div>
        <Weather />
        <Summary />
        <CompleteChecklist />
        <ObliqueStrategies />
      </div>
    )
  }
}

module.exports = Home;
