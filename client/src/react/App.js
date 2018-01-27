import React, { Component } from "react";
import PropTypes from "prop-types";
import { Route, withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { Shortcuts, ShortcutManager } from "react-shortcuts";
import { RoutesMap } from "../redux/navigator";
import { Terminal, Login } from "./meta";
import keymap, { shortcuts } from "./keymap";
import "font-awesome/css/font-awesome.css";

const shortcutManager = new ShortcutManager(keymap);

class App extends Component {
  constructor(props){
    super(props);

    this.state = {
      showTerminal: false
    };
  }

  getChildContext() {
    return {
      shortcuts: shortcutManager
    };
  }

  toggleTerminal(){
    this.setState({showTerminal: !this.state.showTerminal})
  }

  handleShortcuts(action, event) {
    switch (action) {
      case shortcuts.app.toggleTerminal:
        this.toggleTerminal();
        break;
      default:
        console.warn("unknown shortcut");
    }
  }

  render() {
    const { isLoggedIn } = this.props;
    const { showTerminal } = this.state;
    if (!isLoggedIn) {
      return <Login />;
    }
    return (
      <Shortcuts name="APP" handler={(a,e) => this.handleShortcuts(a,e)} global>
        <div>
        <Terminal showTerminal={showTerminal} toggleTerminal={() => this.toggleTerminal() }/>
        {RoutesMap.map(r => {
          return (
            <Route exact path={r.path} key={r.name} component={r.component} />
          );
        })}
        </div>
      </Shortcuts>
    );
  }
}

App.childContextTypes = {
  shortcuts: PropTypes.object.isRequired
};

function mapStateToProps(state, props) {
  const { token } = state.meta;

  return {
    isLoggedIn: token && token.length > 0
  };
}

export default withRouter(connect(mapStateToProps)(App));
