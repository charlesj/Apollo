import React, { Component } from 'react';
var ServerInfo = require('./components/ServerInfo');
var Journal = require('./components/journal/Journal');
import './App.css';

class App extends Component {
  render() {
    return (
      <div>
        <nav className="navbar navbar-toggleable-md navbar-inverse fixed-top bg-inverse">
          <a className="navbar-brand" href="#">Apollo</a>
        </nav>
        <div className="container-fluid">
        <Journal />

      </div>
        <footer className="footer">
           <ServerInfo />
        </footer>
      </div>
      );
  }
}

export default App;
