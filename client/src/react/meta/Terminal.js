import React, { Component } from "react";
import { Link } from "react-router-dom";
import { push } from "react-router-redux";
import { connect } from "react-redux";
import HotKey from "react-shortcut";
import CLI from "terminal-in-react";
import { RoutesMap } from "../../redux/navigator";
import { metaActions } from "../../redux/actions";
import MenuBar from "./MenuBar";
import Notifications from "./Notifications";
import { FlexRow } from "../general";
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
          <FlexRow>
            <CLI
              commands={commands}
              hideTopBar={true}
              backgroundColor="black"
              allowTabs={false}
              watchConsoleLogging={false}
              descriptions={descriptions}
            />
            <Notifications />
            <div>
              <Link to="/">Home</Link>
            </div>
          </FlexRow>
        )}
        <MenuBar
          toggleTerminal={() => {
            this.toggleTerminal();
          }}
        />
      </div>
    );
  }
}

function mapDispatchToProps(dispatch, props) {
  return {
    changePage: path => dispatch(push(path)),
    logout: password => dispatch(metaActions.logout())
  };
}

export default connect(null, mapDispatchToProps)(ApolloTerminal);
