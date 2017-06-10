var React = require('react');
var moment = require('moment');

var apollo = require('../../services/apollo-server');
import { Button, Text } from "@blueprintjs/core";

class LoginSessions extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      sessions: []
    }

    this.formatDate = this.formatDate.bind(this);
    this.loadSessions = this.loadSessions.bind(this);
    this.revokeLoginSession = this.revokeLoginSession.bind(this);
  }

  componentDidMount() {
    this.loadSessions();
  }

  loadSessions() {
    this.setState(() => {
      return {
        entries: []
      }
    });

    apollo.invoke('GetAllActiveLoginSessions', {})
      .then(data => {
        this.setState(() => {
          return {
            sessions: data
          }
        })
      });
  }

  formatDate(date) {
    var converted = moment(date);
    return converted.format('Y-MM-DD HH:mm')
  }

  revokeLoginSession(token) {
    apollo.invoke('RevokeLoginSession', {
      tokenToRevoke: token
    }).then(() => {
      this.loadSessions();
    });
  }

  render() {
    return (<div>
            <h3>Login Sessions</h3>
            <table className="pt-table pt-bordered">
              <thead>
                 <tr>
                <th>id</th>
                <th>token</th>
                <th>created</th>
                <th>last seen</th>
                <th>ip address</th>
                <th>user agent</th>
                <th></th>
                </tr>
              </thead>
              <tbody>
                        { this.state.sessions.map(function(session, index) {
        return (<tr key={session.id}>
                    <td>{session.id}</td>
                    <td>{session.token}</td>
                    <td>{this.formatDate(session.created_at)}</td>
                    <td>{this.formatDate(session.last_seen)}</td>
                    <td>{session.ip_address}</td>
                    <td><Text className='userAgentColumn' ellipsize={true}>{session.user_agent}</Text></td>
                    <td><Button
          className='pt-button pt-intent-warning pt-small'
          iconName="delete"
          text="revoke token"
          onClick={this.revokeLoginSession.bind(null, session.token)}
          /></td>
                      </tr>)
      }, this)}
              </tbody>
            </table>
        </div>)
  }
}


module.exports = LoginSessions;
