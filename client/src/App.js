import React, { Component } from 'react';

var LoginService = require('./services/login-service');

var Login = require('./components/Login');
var ServerInfo = require('./components/ServerInfo');
var Journal = require('./components/journal/Journal');

import './App.css';

class App extends Component {
  loginService = new LoginService();

  constructor(props){
    super(props);

    this.state = {
      loggedIn: false
    }

    this.login = this.login.bind(this);
    this.logout = this.logout.bind(this);
  }

  componentDidMount(){
    this.setState({
      loggedIn: this.loginService.isLoggedIn()
    });
  }

  login(token){
    this.loginService.storeToken(token);
    this.setState({
      loggedIn: this.loginService.isLoggedIn()
    });
  }

  logout(){
    this.loginService.logout();
    this.setState({loggedIn: false})
  }

  render() {
    if(!this.state.loggedIn){
      return (<div><Login onLogin={this.login} /></div>)
    }

    return (
      <div>
        <nav className="navbar navbar-toggleable-md navbar-inverse fixed-top bg-inverse">
          <a className="navbar-brand" href="#">Apollo</a>

        </nav>
        <span onClick={this.logout}>Logout</span>
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
