import React, { Component } from "react";
import { push } from "react-router-redux";
import { connect } from "react-redux";
import HotKey from "react-shortcut";
import { MainRoutes } from "../../redux/navigator";
import { metaActions } from "../../redux/actions";
import MenuBar from "./MenuBar";
import Notifications from "./Notifications";
import QuickNavigator from "./QuickNavigator";
import { FlexRow } from "../_controls";
import "./Terminal.css";

class ApolloTerminal extends Component {
  constructor(props) {
    super(props);

    let commands = MainRoutes.map(route => {
      return {
        command: route.name,
        icon: route.icon,
        label: route.label,
        execute: () => props.changePage(route.path)
      };
    });

    commands.push({
      command: "logout",
      icon: "sign-out",
      label: "Logout",
      execute: () => {
        props.logout();
      }
    });

    this.state = {
      commands,
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
    const { commands, showTerminal } = this.state;
    return (
      <div>
        <HotKey keys={["escape"]} onKeysCoincide={this.toggleTerminal} />
        {showTerminal && (
          <FlexRow>
            <Notifications />
            <QuickNavigator commands={commands} />
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
