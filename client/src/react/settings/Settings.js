import React, { Component } from "react";
import LoginSessions from "./LoginSessions";
import ChangePassword from "./ChangePassword";

class Settings extends Component {
  render() {
    return (
      <div>
        <LoginSessions />
        <ChangePassword />
      </div>
    );
  }
}

export default Settings;
