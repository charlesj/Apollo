import React, { Component } from "react";
import { push } from "react-router-redux";
import { connect } from "react-redux";
import { MainRoutes } from "../../redux/navigator";
import { metaActions } from "../../redux/actions";
import MenuBar from "./MenuBar";
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
    };
  }

  render() {
    const { showTerminal, toggleTerminal } = this.props
    const { commands } = this.state;
    return (
      <div>
        {showTerminal && (
          <FlexRow>
            <QuickNavigator commands={commands} />
          </FlexRow>
        )}
        <MenuBar toggleTerminal={() => toggleTerminal()} />
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
