import React, { Component } from 'react';
var ServerInfo = require('./components/ServerInfo');
import './App.css';

class App extends Component {
  render() {
    return (
        <div>
            <div className='header'>
                <h1>Apollo</h1>
            </div>
            <ServerInfo />
        </div>
    );
  }
}

export default App;
