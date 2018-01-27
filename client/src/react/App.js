import React, { Component } from "react";
import { Route, withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { HotKeys } from "react-hotkeys";
import { RoutesMap } from "../redux/navigator";
import { Terminal, Login } from "./meta";
import "font-awesome/css/font-awesome.css";

const shortcuts = {
  toggleTerminal: "toggleTerminal"
};

const appKeyMap = {
  [shortcuts.toggleTerminal]: "esc"
};

class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      showTerminal: false
    };
  }
  toggleTerminal() {
    this.setState({ showTerminal: !this.state.showTerminal });
  }

  render() {
    const { isLoggedIn } = this.props;
    const { showTerminal } = this.state;

    const shortcutsHandlers = {
      [shortcuts.toggleTerminal]: () => this.toggleTerminal()
    };

    if (!isLoggedIn) {
      return <Login />;
    }
    return (
      <HotKeys keyMap={appKeyMap} handlers={shortcutsHandlers}>
        <div>
          <Terminal
            showTerminal={showTerminal}
            toggleTerminal={() => this.toggleTerminal()}
          />
          {RoutesMap.map(r => {
            return (
              <Route exact path={r.path} key={r.name} component={r.component} />
            );
          })}
        </div>
      </HotKeys>
    );
  }
}

function mapStateToProps(state, props) {
  const { token } = state.meta;

  return {
    isLoggedIn: token && token.length > 0
  };
}

export default withRouter(connect(mapStateToProps)(App));
