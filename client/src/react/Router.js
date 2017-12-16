import React, { Component } from "react";
import { Route } from "react-router-dom";
import { Provider } from "react-redux";
import { ConnectedRouter } from "react-router-redux";
import store, { history } from "../redux/store";
import { RoutesMap } from "../redux/navigator";
import Terminal from "./Terminal";

class Router extends Component {
  render() {
    return (
      <Provider store={store}>
        <ConnectedRouter history={history}>
          <div>
            <Terminal />
            {RoutesMap.map(r => {
              return (
                <Route
                  exact
                  path={r.path}
                  key={r.name}
                  component={r.component}
                />
              );
            })}
          </div>
        </ConnectedRouter>
      </Provider>
    );
  }
}

export default Router;
