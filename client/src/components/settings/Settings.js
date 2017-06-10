var React = require('react');

import { Tab2, Tabs2 } from "@blueprintjs/core";

var LoginSessions = require('./LoginSessions');
var ChangePassword = require('./ChangePassword');

class Settings extends React.Component {
  render() {
    return (
      <div className='container-fluid'>
        <h2>Settings</h2>
        <Tabs2 id="SessionTabs" onChange={this.handleTabChange}>
            <Tab2 id="ls" title="Login Sessions" panel={<LoginSessions />} />
            <Tab2 id="cp" title="Change Password" panel={<ChangePassword />} />
            <Tabs2.Expander />
        </Tabs2>
      </div>
    )
  }
}

module.exports = Settings;
