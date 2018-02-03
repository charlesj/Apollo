import React, { Component } from "react";
import LoginSessions from "./LoginSessions";
import ChangePassword from "./ChangePassword";
import UserSettings from "./UserSettings";

class Settings extends Component {
  render() {
    return (
      <div>
        <LoginSessions />
        <ChangePassword />
        <UserSettings />
      </div>
    );
  }
}

export default Settings;
