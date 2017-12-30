import React, { Component } from "react";
import LoginSessions from "./LoginSessions";
import ChangePassword from "./ChangePassword";

class Settings extends Component {
  render() {
    return (
      <div className="container-fluid">
        <LoginSessions />
        <ChangePassword />
      </div>
    );
  }
}

export default Settings;
