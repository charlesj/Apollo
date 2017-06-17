import React, { Component } from 'react';
var ReactRouter = require('react-router-dom');
var Router = ReactRouter.BrowserRouter;
var Route = ReactRouter.Route;
var Switch = ReactRouter.Switch;

var LoginService = require('./services/login-service');

const Login = require('./components/Login');
const ServerInfo = require('./components/ServerInfo');
const Journal = require('./components/journal/Journal');
const Nav = require('./components/Nav');
const Home = require('./components/Home');
const Settings = require('./components/settings/Settings');
const Health = require('./components/health/Health');

import '../node_modules/gridforms/gridforms/gridforms.css';
import './App.css';

class App extends Component {
  loginService = new LoginService();

  constructor(props) {
    super(props);

    this.state = {
      loggedIn: false
    }

    this.login = this.login.bind(this);
    this.logout = this.logout.bind(this);
  }

  componentDidMount() {
    this.setState({
      loggedIn: this.loginService.isLoggedIn()
    });
  }

  login(token) {
    this.loginService.storeToken(token);
    this.setState({
      loggedIn: this.loginService.isLoggedIn()
    });
  }

  logout() {
    this.loginService.logout();
    this.setState({
      loggedIn: false
    })
  }

  render() {
    if (!this.state.loggedIn) {
      return (<div><Login onLogin={this.login} /></div>)
    }

    return (
      <div>
        <Router>
          <div className='mid'>
            <Nav />
            <div  className='mainContainer'>
            <Switch>
              <Route exact path='/' component={Home} />
              <Route exact path='/journal' component={Journal} />
              <Route exact path='/health' component={Health} />
              <Route exact path='/settings' component={Settings} />
              <Route render={function() {
        return (<p>Not Found</p>)
      }} />
            </Switch>
            </div>
          </div>
        </Router>

        <footer className="footer">
           <ServerInfo /> 
           <a onClick={this.logout}>Logout</a>
        </footer>
      </div>
      );
  }
}

export default App;
