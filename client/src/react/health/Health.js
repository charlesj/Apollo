import React from 'react';
import HealthInput from './HealthInput';
import HomeHealth from './HomeHealth';

class Health extends React.Component {
  render() {
    return (<div><HealthInput />

              <HomeHealth />
    </div>);
  }
}

export default Health;
