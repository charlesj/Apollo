import React, { Component } from "react";
import { Route } from "react-router-dom";
import { connect } from "react-redux";
import { RoutesMap } from "../redux/navigator";
import { Terminal, Login } from "./meta";

import "font-awesome/css/font-awesome.css";

class App extends Component {
  render() {
    const { isLoggedIn } = this.props;
    if (!isLoggedIn) {
      return <Login />;
    }
    return (
      <div>
        <Terminal />
        {RoutesMap.map(r => {
          return (
            <Route exact path={r.path} key={r.name} component={r.component} />
          );
        })}
      </div>
    );
  }
}

function mapStateToProps(state, props) {
  const { token } = state.meta;

  return {
    isLoggedIn: token && token.length > 0
  };
}

export default connect(mapStateToProps)(App);
