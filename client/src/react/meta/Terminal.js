import React, { Component } from "react";
import { push } from "react-router-redux";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import HotKey from "react-shortcut";
import Terminal from "terminal-in-react";
import { RoutesMap } from "../../redux/navigator";
import { meta } from "../../redux/actions";

import "../../../node_modules/terminal-in-react/lib/bundle/terminal-react.css";
import "./Terminal.css";

class ApolloTerminal extends Component {
  constructor(props) {
    super(props);

    let commands = RoutesMap.reduce((reducer, route) => {
      return {
        ...reducer,
        [route.name]: () => props.changePage(route.path)
      };
    }, {});

    commands.logout = () => {
      props.logout();
    };
    let descriptions = RoutesMap.reduce((reducer, route) => {
      return {
        ...reducer,
        [route.name]: `Navigate to ${route.label}`
      };
    }, {});

    this.state = {
      commands,
      descriptions,
      showTerminal: false
    };

    this.toggleTerminal = this.toggleTerminal.bind(this);
  }

  toggleTerminal() {
    this.setState({
      showTerminal: !this.state.showTerminal
    });
  }

  render() {
    const { commands, descriptions, showTerminal } = this.state;
    return (
      <div>
        <HotKey keys={["escape"]} onKeysCoincide={this.toggleTerminal} />
        {showTerminal && (
          <Terminal
            commands={commands}
            hideTopBar={true}
            backgroundColor="black"
            allowTabs={false}
            watchConsoleLogging={false}
            descriptions={descriptions}
          />
        )}
      </div>
    );
  }
}

function mapDispatchToProps(dispatch, props) {
  return {
    ...bindActionCreators({ changePage: path => push(path) }, dispatch),
    logout: password => dispatch(meta.logout())
  };
}

export default connect(null, mapDispatchToProps)(ApolloTerminal);
