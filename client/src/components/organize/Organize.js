import React from 'react';

import Todo from './Todo';
import Queue from './Queue';

class Organize extends React.Component {
  render(){
    return (<div className='organizeContainer'><Todo /><Queue /></div>)
  }
}

module.exports = Organize;
