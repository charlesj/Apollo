import React, { Component, } from 'react'
import { Route, withRouter, } from 'react-router-dom'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { RoutesMap, } from '../redux/navigator'
import { MenuBar, Login, } from './meta/'
import 'font-awesome/css/font-awesome.css'

class App extends Component {
  render() {
    const { isLoggedIn, } = this.props

    if (!isLoggedIn) {
      return <Login />
    }
    return (
      <div>
        <MenuBar />
        {RoutesMap.map(r => {
          return (
            <Route exact path={r.path} key={r.name} component={r.component} />
          )
        })}
      </div>
    )
  }
}

App.propTypes = {
  isLoggedIn: PropTypes.bool.isRequired,
}

function mapStateToProps(state) {
  const { token, } = state.meta

  return {
    isLoggedIn: token && token.length > 0,
  }
}

export default withRouter(connect(mapStateToProps)(App))
