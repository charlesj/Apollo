import React, { Component } from 'react';
var ReactRouter = require('react-router-dom');
var Router = ReactRouter.BrowserRouter;
var Route = ReactRouter.Route;
var Switch = ReactRouter.Switch;

var LoginService = require('./services/login-service');

var Login = require('./components/Login');
var ServerInfo = require('./components/ServerInfo');
var Journal = require('./components/journal/Journal');
var Nav = require('./components/Nav');
var Home = require('./components/Home');
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
          <div className='container-fluid'>
            <Nav />
            <Switch>
              <Route exact path='/' component={Home} />
              <Route exact path='/journal' component={Journal} />
              <Route render={function(){
                     return (<p>Not Found</p>)
              }} />
            </Switch>
          </div>
        </Router>
        <footer className="footer">
           <ServerInfo />&nbsp;
           <a className="danger" onClick={this.logout}>Logout</a>
        </footer>
      </div>
      );
  }
}

export default App;
