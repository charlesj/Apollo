import React, { Component } from 'react';
var ReactRouter = require('react-router-dom');
var Router = ReactRouter.BrowserRouter;
var Route = ReactRouter.Route;
var Switch = ReactRouter.Switch;
import loginService from './services/login-service';
import apolloServer from './services/apollo-server';
import Login from './components/Login';
import Journal from './components/journal/Journal';
import Nav from './components/Nav';
import Home from './components/Home';
import Settings from './components/settings/Settings';
import Health from './components/health/Health';
import Bookmarks from './components/bookmarks/Bookmarks';
import Jobs from './components/jobs/Jobs';
import Finance from './components/financial/Finance';
import Feeds from './components/feeds/Feeds';
import Notebooks from './components/notebooks/Notebooks';
import Boards from './components/boards/Boards';
import Checklists from './components/checklists/Checklists'

import '../node_modules/gridforms/gridforms/gridforms.css';
import '../node_modules/font-awesome/css/font-awesome.css';
import './styles/weather-icons.css';
import './styles/weather-icons-wind.css';
import './App.css';

class App extends Component {
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
      loggedIn: loginService.isLoggedIn()
    });
  }

  login(token) {
    loginService.storeToken(token);
    this.setState({
      loggedIn: loginService.isLoggedIn()
    });
  }

  logout() {
    var token = loginService.getToken();
    apolloServer.invoke('revokeLoginSession', {
      tokenToRevoke: token
    }).then(data => {
      loginService.logout();
      this.setState({
        loggedIn: false
      });
    });
  }

  render() {
    if (!this.state.loggedIn) {
      return (<div><Login onLogin={this.login} /></div>)
    }

    return (
      <div>
        <Router>
          <div className='mid'>
            <Nav logout={this.logout} />
            <div  className='mainContainer'>
            <Switch>
              <Route exact path='/' component={Home} />
              <Route exact path='/boards' component={Boards} />
              <Route exact path='/journal' component={Journal} />
              <Route exact path='/notebooks' component={Notebooks} />
              <Route exact path='/checklists' component={Checklists} />
              <Route exact path='/health' component={Health} />
              <Route exact path='/financial' component={Finance} />
              <Route exact path='/bookmarks' component={Bookmarks} />
              <Route exact path='/feeds' component={Feeds} />
              <Route exact path='/jobs' component={Jobs} />
              <Route exact path='/settings' component={Settings} />
              <Route render={function() {
        return (<p>Not Found</p>)
      }} />
            </Switch>
            </div>
          </div>
        </Router>
      </div>
      );
  }
}

export default App;
