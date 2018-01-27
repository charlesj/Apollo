import React, { Component } from "react";
import { FlexRow, TextButton } from "../_controls";
import ServerActivity from "./ServerActivity";
import ApplicationInfo from "./ApplicationInfo";
import "./MenuBar.css";

class MenuBar extends Component {
  render() {
    const {
      toggleTerminal,
    } = this.props;
    return (
      <FlexRow className="menuBar">
        <ServerActivity />
        <TextButton onClick={toggleTerminal}>Terminal</TextButton>
        <ApplicationInfo />
      </FlexRow>
    );
  }
}

export default MenuBar;
